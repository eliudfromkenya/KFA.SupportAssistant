using Humanizer;
using Microsoft.EntityFrameworkCore;
using PPMS.Console.Data;
using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PPMS.Console.Generators
{
    static class GraphQlGenerators
    {
        static readonly string GraphQlPath = Path.Combine(Defaults.MainPath, "GraphQls");
        internal static void Generate()
        {
            if (!Directory.Exists(GraphQlPath))
                Directory.CreateDirectory(GraphQlPath);

            GenerateQueryHeader();
            GenerateQuery();
            GenerateInputFields();
            GenerateMutations();
            GenerateQueryFields();
        }

        private static void GenerateMutations()
        {
            var subQueryPath = Path.Combine(GraphQlPath, "Mutations");
            using var db = new Context();
            if (!Directory.Exists(subQueryPath))
                Directory.CreateDirectory(subQueryPath);

            var body = $@"using GraphQL.Types;
using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes.InputTypes;
using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes.QueryTypes;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using Pilgrims.Projects.Assistant.DataLayer.Data.Mutations;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.Mutations
{{
    public class KfaDataMutation : ObjectGraphType
    {{
        public KfaDataMutation(DataContext dbContext)
        {{
{string.Join("\r\n", db.Tables.Select(x => x.Name).ToArray().Select(x =>
 {
     var singular = Functions.Singularize(x).MakeName();
     return $@"            new RecordMutator<{singular}, {singular}QueryType, {singular}InputType>(this, dbContext, ""{x}"");";
 }))}
        }}
    }}
}}
";


            var path = Path.Combine(subQueryPath, "KfaDataMutation.cs");
            File.WriteAllText(path, body.ToString());
        }

        private static void GenerateQueryFields()
        {
            var subQueryPath = Path.Combine(GraphQlPath, "GraphQLTypes", "QueryTypes");
            using var db = new Context();
            if (!Directory.Exists(subQueryPath))
                Directory.CreateDirectory(subQueryPath);

            foreach (var table in db.Tables)
            {
                if (string.IsNullOrWhiteSpace(table?.Name))
                    continue;

                var list = new List<string>();
                var name = table.Name.MakeName();
                var singular = Functions.Singularize(table.Name).MakeName();

                var allRels = db.Relations.ToArray()
                   .Distinct(new RelationComparer())
                   .Where(x => x != null)
                   .Select(x =>
                   {

                       DatabaseTable master = null, foreign = null;
                       try
                       {
                           foreign = db.Tables.FirstOrDefault(y => y.Id == db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId).TableId);
                       }
                       catch { }
                       try
                       {
                           master = db.Tables.FirstOrDefault(y => y.Id == db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId).TableId);
                       }
                       catch { }

                       return new
                       {
                           Rel = x,
                           Master = master,
                           Foreign = foreign,
                           MasterColumn = db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId)?.ColumnName,
                           ForeignColumn = db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)?.ColumnName
                       };
                   }).Where(x => x.Master != null && x.Foreign != null).ToArray();



                var body = new StringBuilder($@"using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLQueries.SubQueries;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes.QueryTypes
{{
    internal class {singular}QueryType :  ObjectGraphType<{singular}>
    {{
        public {singular}QueryType()
        {{
");

                var cols = db.Columns.Where(x => x.TableId == table.Id).ToArray();
                var data = cols.Select(x =>
                {
                    var sb = new StringBuilder();
                    string ans;
                    if (x.IsPrimary)
                        ans = $@"            Field(x => x.Id);";
                    else if (x.IsNullable)
                        ans = $@"            Field(x => x.{x.ColumnName.MakeName()}, nullable: true);";
                    else
                        ans = $@"            Field(x => x.{x.ColumnName.MakeName()});";

                    sb.AppendLine(ans);

                    var rels = allRels.Where(y => y.Rel.MasterColumnId == x.Id || y.Rel.ForeignColumnId == x.Id).ToArray();

                    foreach (var rel in rels)
                    {
                        var colName = Functions
                        .MakeAllFirstLetterCapital(x.StrimLinedName, false)
                        .Replace(" ", "");

                        var propName = x.ColumnName;
                        if (Regex.IsMatch(propName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
                        {
                            var len = Regex.Match(propName,
                                      "(id|code|no|number|key) *$",
                                      RegexOptions.IgnoreCase).Value.Length;
                            propName = propName.Trim().Substring(0, propName.Length - len);
                            propName = propName.MakeName();
                        }
                        if ((rel.ForeignColumn == rel.MasterColumn) &&
                        (rel.Master.Name == rel.Foreign.Name))
                        {
                            var pName = propName;
                            for (int i = 0; i <10; i++)
                            {
                                if (list.Contains(pName))
                                {
                                    pName = propName + (i + 1);
                                    continue;
                                }
                                else
                                {
                                    list.Add(propName = pName);
                                    break;
                                }
                            }
                            var type = Functions.Singularize(rel.Master.Name);
                            sb.AppendLine($@"
            Field<{type.MakeName()}QueryType>(""{propName}"",
                arguments: WebExtensions.GetSingularQueryArguments(),
                resolve: context => context.Source.{propName});");
                        }
                        else if (rel.Master.Id == rel.Foreign.Id)
                        {
                            //string condition;
                            //var isMaster = rel.Master.PrimaryKey.ColumnName == rel.MasterColumn;
                            //if (!isMaster)
                            //    condition = $"x => x.{ rel.MasterColumn.MakeName()} == context.Source.Id";
                            //else
                            //    condition = $"x => x.Id == context.Source.{ rel.ForeignColumn.MakeName()}";

                            var pName = propName;
                            for (int i = 0; i <10; i++)
                            {
                                if (list.Contains(pName))
                                {
                                    pName = propName + (i + 1);
                                    continue;
                                }
                                else
                                {
                                    list.Add(propName = pName);
                                    break;
                                }
                            }
                            var type = Functions.Singularize(rel.Master.Name);
                            sb.AppendLine($@"
            Field<{type.MakeName()}QueryType>(""{propName}"",
                arguments: WebExtensions.GetSingularQueryArguments(),
                resolve: context => context.Source.{propName});");
                            //                sb.AppendLine($@"
                            //Field<{type.MakeName()}QueryType>(""{propName}"",
                            //    arguments: WebExtensions.GetSingularQueryArguments(),
                            //    resolve: context =>
                            //    {{
                            //        var query = new Repository<{type.MakeName()}>().Get();
                            //        return query.Filter(context.Arguments)
                            //               .FirstOrDefault({condition});
                            //    }});");
                            //{(rel.Foreign.PrimaryKey.ColumnName == rel.ForeignColumn? "Id": rel.ForeignColumn.MakeName())}
                        }
                        else if (rel.Master.Id == table.Id)
                        {
                            propName = rel.Foreign.Name;

                            if (rels.Count() > 1 && rel.ForeignColumn != rel.MasterColumn)
                            {
                                propName = rel.ForeignColumn;
                                if (Regex.IsMatch(propName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
                                {
                                    var len = Regex.Match(propName,
                                        "(id|code|no|number|key) *$",
                                        RegexOptions.IgnoreCase).Value.Length;

                                    propName = propName.Trim().Substring(0, propName.Length - len);
                                }
                            }
                            propName = propName.MakeName();

                            if (!propName.EndsWith("s"))
                                propName = propName + "s";

                            string condition;
                            var isMaster = rel.Master.PrimaryKey.ColumnName == rel.MasterColumn;
                            if (isMaster)
                                condition = $"x => x.{ rel.ForeignColumn.MakeName()} == context.Source.Id";
                            else
                                condition = $"x => x.Id == context.Source.{ rel.MasterColumn.MakeName()}";

                            var pName = propName;
                            for (int i = 0; i <10; i++)
                            {
                                if (list.Contains(pName))
                                {
                                    pName = propName + (i + 1);
                                    continue;
                                }
                                else
                                {
                                    list.Add(propName = pName);
                                    break;
                                }
                            }

                            propName = rel.Foreign.Name.MakeName();
                            var type = Functions.Singularize(rel.Foreign.Name);
                            sb.AppendLine($@"
            Field<ListGraphType<{type.MakeName()}QueryType>>(""{propName}"",
                arguments: WebExtensions.GetQueryArguments(),
                resolve: context =>
                {{
                    var query = {rel.Foreign.Name.MakeName()}Query.DbQuery;
                    return query.Filter(context.Arguments).Where({condition});
                }});");
                        }
                        else
                        {

                            string condition;
                            var isMaster = rel.Master.PrimaryKey.ColumnName == rel.MasterColumn;
                            if (!isMaster)
                                condition = $"x => x.{ rel.MasterColumn.MakeName()} == context.Source.Id";
                            else
                                condition = $"x => x.Id == context.Source.{ rel.ForeignColumn.MakeName()}";

                            var pName = propName;
                            for (int i = 0; i <10; i++)
                            {
                                if (list.Contains(pName))
                                {
                                    pName = propName + (i + 1);
                                    continue;
                                }
                                else
                                {
                                    list.Add(propName = pName);
                                    break;
                                }
                            }

                            var type = Functions.Singularize(rel.Master.Name);
                            sb.AppendLine($@"
            Field<{type.MakeName()}QueryType>(""{propName}"",
                arguments: WebExtensions.GetSingularQueryArguments(),
                resolve: context => context.Source.{propName});");
                        }
                    }
                    return sb.ToString();
                });

                body.AppendJoin("\r\n", data);

                body.Append(@"}
   }
}

");
                var path = Path.Combine(subQueryPath, $"{table.Name.MakeName()}QueryType.cs");
                File.WriteAllText(path, body.ToString());
            }
        }

        private static void GenerateInputFields()
        {
            var subQueryPath = Path.Combine(GraphQlPath, "GraphQLTypes", "InputTypes");
            using var db = new Context();
            if (!Directory.Exists(subQueryPath))
                Directory.CreateDirectory(subQueryPath);

            var relCols = db.Relations
                .ToArray()
                .SelectMany(x => new[] { x.ForeignColumnId, x.MasterColumnId })
                .Distinct().ToArray();

            foreach (var table in db.Tables)
            {
                if (string.IsNullOrWhiteSpace(table?.Name))
                    continue;

                var name = table.Name.MakeName();
                var singular = Functions.Singularize(table.Name).MakeName();

                var body = new StringBuilder($@"using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes.InputTypes
{{
    internal class {singular}InputType : InputObjectGraphType
    {{
        public {singular}InputType()
        {{
");

                var cols = db.Columns.Where(x => x.TableId == table.Id).ToArray();
                var data = cols.Select(x =>
                {
                    var type = x.Type;
                    if (type?.Length > 1)
                        type = $"{type[0].ToString().ToUpper()}{type.Substring(1)}GraphType";

                    switch (x.Type.ToLower())
                    {
                        case "bool":
                            type = "BooleanGraphType";
                            break;
                        case "double":
                            type = "FloatGraphType";
                            break;
                        case "byte[]":
                            type = "ListGraphType<ByteGraphType>";
                            break;
                        default:
                            break;
                    }
                    if (relCols.Contains(x.Id))
                        type = "StringGraphType";

                    string ans;
                    if (x.IsPrimary)
                        ans = $@"            Field<StringGraphType>(""Id"");";
                    else if (x.IsNullable)
                        ans = $@"            Field<NonNullGraphType<{type}>>(""{x.ColumnName.MakeName()}"");";
                    else
                        ans = $@"            Field<{type}>(""{x.ColumnName.MakeName()}"");";
                    return ans.Replace("Global::System.DateTimeGraphType", "DateTimeGraphType")
                    .Replace("SingleGraphType", "ShortGraphType");
                });

                body.AppendJoin("\r\n", data);

                body.Append(@"}
      }
    }

");

                var path = Path.Combine(subQueryPath, $"{table.Name.MakeName()}InputType.cs");
                File.WriteAllText(path, body.ToString());
            }
        }

        private static void GenerateQueryHeader()
        {
            using var db = new Context();
            var body = $@"using GraphQL.Types;
using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLQueries.SubQueries;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLQueries
{{
    public class KfaDataQuery : ObjectGraphType<object>
    {{
        public KfaDataQuery(DataContext dbContext)
        {{
            Name = ""KfaSystemQueries"";
{
              string.Join("\r\n", db.Tables.Select(x => x.Name)
              .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()
              .Select(x => $"            new {x.MakeName()}Query(this);"))}
        }}
    }}
}}
";
            var path = Path.Combine(GraphQlPath, "KfaDataQuery.cs");
            File.WriteAllText(path, body);


            body = $@"using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes.InputTypes;
using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes.QueryTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Pilgrims.Projects.Assistant.DataLayer.Data
{{
    static class IOCRegister
    {{
        internal static void Register(IServiceCollection services)
        {{
            //services.AddSingleton<OrderBatchHeaderType>();
            //services.AddSingleton<OrderBatchHeaderMutation>();
            //services.AddSingleton<OrderBatchHeaderInputType>();
            //services.AddSingleton<OrderDocumentType>();
{
   string.Join("\r\n", db.Tables.Select(x =>
 string.Format(@"            services.AddSingleton<{0}InputType>();
            services.AddSingleton<{0}QueryType>();", Functions.Singularize(x.Name).MakeName())))
}        
        }}
    }}
}}
";
            path = Path.Combine(GraphQlPath, "IOCRegister.cs");
            File.WriteAllText(path, body);
        }

        private static void GenerateQuery()
        {
            var subQueryPath = Path.Combine(GraphQlPath, "SubQueries");
            using var db = new Context();
            if (!Directory.Exists(subQueryPath))
                Directory.CreateDirectory(subQueryPath);

            var allRels = db.Relations.ToArray()
                 .Distinct(new RelationComparer())
                 .Where(x => x != null)
                 .Select(x =>
                 {

                     DatabaseTable master = null, foreign = null;
                     try
                     {
                         foreign = db.Tables.FirstOrDefault(y => y.Id == db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId).TableId);
                     }
                     catch { }
                     try
                     {
                         master = db.Tables.FirstOrDefault(y => y.Id == db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId).TableId);
                     }
                     catch { }

                     return new
                     {
                         Rel = x,
                         Master = master,
                         Foreign = foreign,
                         MasterColumn = db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId)?.ColumnName,
                         ForeignColumn = db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)?.ColumnName
                     };
                 }).Where(x => x.Master != null && x.Foreign != null).ToArray();


            foreach (var table in db.Tables)
            {
                if (string.IsNullOrWhiteSpace(table?.Name))
                    continue;

                var rels = allRels.Where(x => x.Foreign.Name == table.Name).ToArray();

                var name = table.Name.MakeName();
                var singular = Functions.Singularize(table.Name).MakeName();

                var body = new StringBuilder($@" using global::System.Linq;
    using GraphQL.Types;
    using Pilgrims.Projects.Assistant.Data.Helpers;
    using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes.QueryTypes;
    using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLTypes;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.GraphQLQueries.SubQueries
{{
    class {name}Query
    {{
        /// <summary>
        /// Gets the DbQuery.
        /// </summary>
        public static IQueryable<{singular}> DbQuery
        {{
            get
            {{
                static IQueryable<{singular}> CheckQuery(IQueryable<{singular}> query)
                {{
                     query = query
                     //.Include(x => x.RecordCommentBase.RecordComments)
                     //.Include(x => x.RecordVerificationBase.RecordVerifications){string.Join("", rels.Select(x => GetPropertyName(x.ForeignColumn))
.Select(x => $"\r\n                    .Include(x => x.{x.MakeName()})").Distinct())};
                    return query;
                }}
                return new Repository<{singular}>().Get(CheckQuery);
            }}
        }}

        /// <summary>
        /// Initializes a new instance of the <see cref=""{name}Query""/> class.
        /// </summary>
        /// <param name=""dataQuery"">The dataQuery<see cref=""KfaDataQuery""/>.</param>
                internal {name}Query(KfaDataQuery dataQuery)
                {{
                    dataQuery.Field<ListGraphType<{singular}QueryType>>(""{name}"", 
                              resolve: ctx => DbQuery.InitializeQuery(ctx));
                    
                    dataQuery.Field<ListGraphType<{singular}QueryType>>(""{name}ByFilter"",
                              arguments: new QueryArguments()
                              {{
                                   new QueryArgument<IdQueryInputType>
                                   {{
                                        Name = ""param"",
                                        Description = ""Filter Parameter""
                                   }}
                              }},
                              resolve: x => x.GetModels(DbQuery));
               ");

                //    var cols = db.Columns.Where(x => x.TableId == table.Id).ToArray();
                //    foreach (var rel in db.Relations
                //        .Include(x => x.ForeignColumn.Table)
                //        .Include(x => x.MasterColumn.Table)
                //        .Where(x => cols.Select(y => y.Id).Contains(x.ForeignColumnId)))
                //    {
                //        var master = Functions.Singularize(rel.MasterColumn.Table.Name).MakeName();
                //        body.AppendLine($@"

                //dataQuery.Field<ListGraphType<{master}QueryType>>(""{rel.MasterColumn.Table.Name.MakeName()}By{rel.ForeignColumn.ColumnName.MakeName()}"" ,
                //arguments: new QueryArguments()
                //{{
                //        new QueryArgument<StringGraphType>
                //        {{
                //            Name = ""Id"",
                //            Description = ""{rel.ForeignColumn.ColumnName}""
                //        }}
                //}},
                //resolve: ctx => dbContext.{rel.MasterColumn.Table.Name.MakeName()}.Where(x => x.{rel.ForeignColumn.ColumnName.MakeName()} == ctx.Arguments[""Id""].ToString())); ");
                //    }


                //    foreach (var rel in db.Relations
                //      .Include(x => x.ForeignColumn.Table)
                //      .Include(x => x.MasterColumn.Table)
                //      .Where(x => cols.Select(y => y.Id).Contains(x.MasterColumnId)))
                //    {
                //        var foreign = Functions.Singularize(rel.ForeignColumn.Table.Name).MakeName();
                //        body.AppendLine($@"

                //dataQuery.Field<{foreign}QueryType>(""{foreign}By{rel.MasterColumn.ColumnName.MakeName()}"" ,
                //arguments: new QueryArguments()
                //{{
                //        new QueryArgument<StringGraphType>
                //        {{
                //            Name = ""Id"",
                //            Description = ""{rel.MasterColumn.ColumnName}""
                //        }}
                //}},
                //resolve: ctx => dbContext.{rel.ForeignColumn.Table.Name.MakeName()}.FirstOrDefault(x => x.{rel.ForeignColumn.ColumnName.MakeName()} == ctx.Arguments[""Id""].ToString())); ");
                //    }


                body.Append(@"
       }
     }
    }

");

                var path = Path.Combine(subQueryPath, $"{table.Name.MakeName()}Query.cs");
                File.WriteAllText(path, body.ToString());
            }
        }

        private static string GetPropertyName(string masterColumn)
        {
            var propName = masterColumn;
            if (Regex.IsMatch(propName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
            {
                var len = Regex.Match(propName,
                          "(id|code|no|number|key) *$",
                          RegexOptions.IgnoreCase).Value.Length;
                propName = propName.Trim().Substring(0, propName.Length - len);
            }
            return propName.MakeName();
        }
    }
}

