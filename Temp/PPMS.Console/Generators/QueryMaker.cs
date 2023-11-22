using Humanizer;
using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PPMS.Console.Generators
{
    static class QueryMaker
    {
        internal static void CreateModelQuery1(IEnumerable<DatabaseTable> tables, IEnumerable<TableRelation> rels)
        {
            string getCaptions(DatabaseTable table)
            {
                return string.Join("", rels.Where(x => table.Columns
                .Select(y => y.Id).Contains(x.ForeignColumnId))
                .Select(x =>
                {
                    var propName = x.ForeignColumn.ColumnName;
                    var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                        propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                    propName = propName.MakeName();

                    var tt = string.Join(@" + "" - "" + ", Templates.GetTableMasters(x.MasterColumn.Table)
                    .Select(d => $@"ol.{propName}.{(d.IsPrimary ? "Id" : d.ColumnName.MakeName())}"));
                    return $@"
                           .ForMember(
                              model => model.{propName}_Caption,
                              qModel => qModel.MapFrom(ol => {tt}))";
                }));
            }

            string getBody(DatabaseTable table)
            {
                return $@"                    #region {table.Name}
                    case ""{table.Name.MakeName().Camelize()}"":
                        {{
                          return await new Repository<{ table.Name.MakeName().Singularize() }>()
                                .Get(CheckQuery)
                                .ProjectTo<{table.Name.MakeName().Singularize()}QueryModel>(new MapperConfiguration(cfg => cfg.CreateMap<{table.Name.MakeName().Singularize()}, {table.Name.MakeName().Singularize()}QueryModel>(){ getCaptions(table) }
                           )).ToListAsync();
    }}
                    #endregion";
            }

            string MxUsd = $@"using AutoMapper;
using AutoMapper.QueryableExtensions;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Data.Helpers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.Classes
{{
    static class ModelQueries
    {{
        internal async static Task<dynamic> GetObjects(string subUrl, Contracts.Helpers.PageListParams param = null)
        {{
            try
            {{
                subUrl = subUrl.MakeName().ToLower();
                var db = DataContext.Create();
                IQueryable<T> CheckQuery<T>(IQueryable<T> query) where T : BaseModel
                {{
                    if (param != null)
                        query = query.CheckFilters(param);
                    return query;
                }}
                switch (subUrl)
                {{
{ string.Join("\r\n\r\n", tables.Select(x => getBody(x))) }

                    default:
                        break;
                }}
return new {{ }};
            }}
            catch (Exception ex)
{{
    throw ex.InnerError();
}}
        }}
    }}
}}
";

            var path = Path.Combine(Defaults.MainPath, "General");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "QueryModel.cs"), MxUsd);
        }


        internal static void CreateModelQuery(IEnumerable<DatabaseTable> tables, IEnumerable<TableRelation> rels)
        {
            string getCaptions(DatabaseTable table)
            {
                return string.Join("", rels.Where(x => table.Columns
                .Select(y => y.Id).Contains(x.ForeignColumnId))
                .Select(x =>
                {
                    var propName = x.ForeignColumn.ColumnName;
                    var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                        propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                    propName = propName.MakeName();

                    var tt = string.Join(@" + "" - "" + ", Templates.GetTableMasters(x.MasterColumn.Table)
                    .Select(d => $@"ol.{propName}.{(d.IsPrimary ? "Id" : d.ColumnName.MakeName())}"));
                    return $@"
                           .ForMember(
                              model => model.{propName}_Caption,
                              qModel => qModel.MapFrom(ol => {tt}))";
                }));
            }

            string getBody(DatabaseTable table)
            {
                return $@"                    #region {table.Name}
                        if (typeof(T) == typeof({ table.Name.MakeName().Singularize() }))
                        {{
                          return (IQueryable<T>)new Repository<{ table.Name.MakeName().Singularize() }>()
                                .Get(CheckQuery)
                                .ProjectTo<{table.Name.MakeName().Singularize()}QueryModel>(new MapperConfiguration(cfg => cfg.CreateMap<{table.Name.MakeName().Singularize()}, {table.Name.MakeName().Singularize()}QueryModel>(){ getCaptions(table) }
                           ));
    }}
                    #endregion";
            }

            string MxUsd = $@"using AutoMapper;
using AutoMapper.QueryableExtensions;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Data.Helpers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.Classes
{{
    static class ModelQueries
    {{
        internal async static Task<IQueryable<T>> GetObjects<T>(Contracts.Helpers.PageListParams param = null) where T : BaseQueryModel, new()
        {{
            try
            {{
                var db = DataContext.Create();
                IQueryable<Tx> CheckQuery<Tx>(IQueryable<Tx> query) where Tx : BaseModel
                {{
                    if (param != null)
                        query = query.CheckFilters(param);
                    return query;
                }}
              
{string.Join("\r\n\r\n", tables.Select(x => getBody(x))) }
              
              throw new BadImageFormatException($""Can't translate the query to type {{typeof(T).FullName}}"");
     
                }}
            catch (Exception ex)
{{
    throw ex.InnerError();
}}
        }}
    }}
}}
";

            var path = Path.Combine(Defaults.MainPath, "General");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "QueryModel.cs"), MxUsd);
        }

    }
}

