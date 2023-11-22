using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using PPMS.Console.Models;

namespace PPMS.Console.Generators;
  public class EfDbContext
  {

    internal static void GenerateContext()
    {
      var sb = new StringBuilder(@"
      
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Pilgrims.Projects.Assistant.DataLayer.DataModels;

namespace Pilgrims.Projects.Assistant.DataLayer {
  public class DatabaseContext : DbContextWithTriggers {
");
      using (var db = new Data.Context())
      {

        var tables = from table in db.Tables
                     from grp in db.Groups
                     where table.Id == grp.TableId
                     select new
                     {
                       Group = grp.GroupName,
                       table.Name
                     };

        //var grps = tables.Select(x => x.Group).Distinct().ToArray()
        //    .Select(x =>
        //    {
        //        var spls = x.Replace("/", "\\").Split('\\').Select(m => m.MakeName());
        //        return "using Pilgrims.Projects.Assistant.DataLayer." + string.Join(".", spls) + ";";
        //    }).ToArray();

        var objs = tables.Select(x => x.Name)
            .Select(x => string.Format("    public DbSet<{0}> {1} {{ get; set; }}",
               Functions.Singularize(x).MakeName(), x.MakeName()
           )).Distinct().ToArray();

        var mm = string.Join("\r\n", objs);
        //sb.Insert(0, string.Join("\r\n", grps));
        sb.Append(mm);
      }
      sb.Append(@"
      
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlite (@""Filename=XData.db"");
    }
    protected override void OnModelCreating (ModelBuilder builder) {
      base.OnModelCreating (builder);

      // builder.Entity<Like>()
      //     .HasOne(u => u.Liker)
      //     .WithMany(u => u.Likees)
      //     .HasForeignKey(u => u.LikerId)
      //     .OnDelete(DeleteBehavior.Restrict);
      // builder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);
    }
  }
}");

      var path = Path.Combine(Defaults.MainPath, "Data");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      path = Path.Combine(path, "DatabaseContext.cs");
      File.WriteAllText(path, sb.ToString());
    }

    internal static void Generate()
    {
      using (var db = new Data.Context())
      {
        var rel = db.Relations.Select(x => x.MasterColumnId).Union(db.Relations.Select(x => x.ForeignColumnId)).Distinct();
        var mCols = db.Columns.Where(x => rel.Contains(x.Id)).Select(x => new { x.ColumnName, x.Id }).Distinct().ToArray();
        var updateCols = mCols.Where(x => !Regex.IsMatch(x.ColumnName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
            .Select(x => new
            {
              x.Id,
              Name = x.ColumnName + " ID"
            }).ToArray();

        foreach (var item in updateCols)
          db.Columns.First(x => x.Id == item.Id).ColumnName = item.Name;

        updateCols = mCols.Where(x => Regex.IsMatch(x.ColumnName, "id +id *$", RegexOptions.IgnoreCase)).Select(x => new
        {
          Id = x.Id,
          Name = x.ColumnName.Substring(0, x.ColumnName.Length - 2).Trim()
        }).ToArray();

        foreach (var item in updateCols)
          db.Columns.First(x => x.Id == item.Id).ColumnName = item.Name;

        db.SaveChanges();
      }
      using var tsk = Task.Factory.StartNew(GenerateModels);
      using var tsk2 = Task.Factory.StartNew(GenerateContext);
      using var tsk1 = Task.Factory.StartNew(GenerateEfQueries);
      using var tsk3 = Task.Factory.StartNew(SeedData.GenerateContext);
      Task.WaitAll(tsk, tsk1, tsk2, tsk3);
      System.Console.WriteLine("Done");
    }


    internal static void GenerateModels()
    {
      try
      {
        using var db = new Data.Context();
        var tables = db.Tables;
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
                ForeignColumn = db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)?.ColumnName,
                MasterTableColumn = db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId),
                ForeignTableColumn = db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)
              };
            }).Where(x => x.Master != null && x.Foreign != null).ToArray();
        var dbGroups = db.Groups.ToArray();
        var dbColumns = db.Columns.ToArray();

        var contextConfigs = new Dictionary<string, List<Tuple<string, string>>>();
        //int tableCount = 1;
        var sbForeign = new StringBuilder();


        var relColIds = allRels.SelectMany(x => new[] { x.Rel.ForeignColumnId, x.Rel.MasterColumnId }).Distinct().ToArray();

        foreach (var table in tables)
        {
          //System.Console.WriteLine("Processing table " + table.Name + " => " + tableCount++ + " / " + tables.Count());
          var groupName = "General";
          var columns = dbColumns.Where(x => x.TableId == table.Id).ToArray();
          var groupObj = dbGroups.FirstOrDefault(x => x.TableId == table.Id);
          if (groupObj != null &&
              !string.IsNullOrWhiteSpace(groupObj.GroupName))
            groupName = groupObj.GroupName;

          var importGroups = new List<string>();

          var group = groupName.Replace("/", "\\").Split('\\')
              .Select(x => Functions.MakeAllFirstLetterCapital(x, false).Replace(" ", ""))
              .ToArray();

          var name = Functions.MakeAllFirstLetterCapital(table.StrimLinedName, false);
          var singular = Functions.Singularize(name)?.Replace(" ", "");
          var tblName = $"tbl_{name?.Replace(" ", "_")?.ToLower()}";

          var sb = new StringBuilder();
          var sbModelInterface = new StringBuilder();
        var sbModelDTO = new StringBuilder();
        var sbImplicitConvertionModels = new List<String>();
          var sbImplicitConvertionMasterMethods = new List<String>();

        sbModelDTO.Append($@"
using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.DynamicsAssistant.Core.DataLayer.Models;
namespace KFA.DynamicsAssistant.Infrastructure.DTOs;
public record class {singular.MakeName()}DTO : BaseDTO<I{singular.MakeName()}Model>
{{").AppendLine();


        sbModelInterface.Append($@"using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using KFA.DynamicsAssistant.Core.DataLayer;

namespace KFA.DynamicsAssistant.Core.DataLayer.Models;

    public interface I{singular.MakeName()}Model : IBaseModel
    {{
").AppendLine();

        sb.AppendFormat(@"using KFA.DynamicsAssistant.Core.DataLayer.Models;
using ReactiveUI;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReactiveUI.Fody.Helpers;
using KFA.DynamicsAssistant.Infrastructure.Data;
using KFA.DynamicsAssistant.Infrastructure.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFA.DynamicsAssistant.Infrastructure.Models;
  [Table (""{0}"")]
  internal sealed record class {1}: BaseModel, I{1}Model {{
  public override string? ___tableName___ {{ get; protected set; }} = ""{0}"";",
                  tblName, singular, string.Join(".", group))
              .AppendLine();

          foreach (var column in columns)
          {
            var rels = allRels.Where(x => x.Rel.MasterColumnId == column.Id || x.Rel.ForeignColumnId == column.Id).ToArray();
            var colName = Functions.MakeAllFirstLetterCapital(column.StrimLinedName, false)
                .Replace(" ", "");

            if (!column.IsPrimary)
              sbImplicitConvertionModels.Add($"                {colName} = obj.{colName}");

            if (column.IsUnique)
            {
              string builder;
              if (column.IsPrimary)
                builder = string.Format(@"  builder.Entity<{0}>()
        .HasIndex(b => b.Id)
        .IsUnique();", singular, colName);
              else
                builder = string.Format(@"  builder.Entity<{0}>()
        .HasIndex(b => b.{1})
        .IsUnique();", singular, colName);

              if (contextConfigs.ContainsKey(group[0]))
              {
                contextConfigs[group[0]].Add(new Tuple<string, string>(builder, groupName));
              }
              else
              {
                contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, groupName) });
              }
            }

            if (!column.IsNullable)
              sb.Append("    [Required]").AppendLine();
            if (column.IsUnique)
              sb.Append("//    [Index(IsUnique = true)]").AppendLine();


            //          if (column.Type.ToLower().Contains("double")
            //              || column.Type.ToLower().Contains("decimal"))
            //          {

            //              var builder = string.Format(@" builder.Entity<{0}>()
            //.Property(t => t.{1})
            //.HasPrecision(20,5);", singular, colName);

            //              if (contextConfigs.ContainsKey(group[0]))
            //              {
            //                  contextConfigs[group[0]].Add(new Tuple<string, string>(builder, groupName));
            //              }
            //              else
            //              {
            //                  contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, groupName) });
            //              }
            //          }


            if (!column.IsPrimary && relColIds.Contains(column.Id))
            {

              var builder = string.Format(@" builder.Entity<{0}>()
              .Property(t => t.{1})
              .HasMaxLength(BaseModel.___PrimaryMaxLength___);", singular, colName);

              if (contextConfigs.ContainsKey(group[0]))
              {
                contextConfigs[group[0]].Insert(0, new Tuple<string, string>(builder, groupName));
              }
              else
              {
                contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, groupName) });
              }
            }
            else if (!column.IsPrimary)
            {
              if (column.Length > 3 && column.Length < 20000)
                sb.AppendFormat(@"    [MaxLength({0}, ErrorMessage=""Please {1} must be {0} characters or less"")]", column.Length, column.ColumnName.ToLower()).AppendLine();
            }

            sb.AppendFormat(@"    [Column (""{0}"")]", column.ColumnName.Replace(" ", "_").ToLower()).AppendLine();
            if (column.IsPrimary)
            {
              var prop = @"    public override string? Id { get; init; }
";
              sb.AppendLine(prop);
            }
            else if (rels.Any())
            {
              var small = Functions.MakeFirstSmallOtherLetterCapital(colName, false);
              var prop = string.Format(@"        public string? {0} {{ get; init; }}
", colName);

              sb.Append(prop).AppendLine();
              sbModelInterface.Append(@"        string? " + colName + " { get; set; }").AppendLine();
            sbModelDTO.Append(@"        public string? " + colName + " { get; set; }").AppendLine();

            foreach (var rel in rels)
              {
                var propName = colName;
                if (Regex.IsMatch(propName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
                {
                  var len = Regex.Match(propName,
                            "(id|code|no|number|key) *$",
                            RegexOptions.IgnoreCase).Value.Length;
                  propName = propName.Trim().Substring(0, propName.Length - len);
                  propName = propName.MakeName();
                }

                if (columns.Any(x => x.Id == rel.Rel.ForeignColumnId)
                    && columns.Any(x => x.Id == rel.Rel.MasterColumnId))
                {
                  var attrib = string.Format(@"    [ForeignKey(nameof({0}))]", colName);
                  if (!sb.ToString().Contains(attrib))
                  {
                    var type = Functions.Singularize(rel.Master.Name);
                    sb.Append(attrib).AppendLine();
                    sb.AppendFormat("    public {0}? {1} {{ get; set; }}", type.MakeName(), propName).AppendLine();
                    sbModelInterface.AppendFormat("        string? {1}_Caption {{ get; set; }}", type.MakeName(), propName).AppendLine();

                    sb.AppendFormat(@" //[Reactive, NotMapped]
        public string? {1}_Caption {{ get; set; }}
", type.MakeName(), propName).AppendLine();

                    //sb.Append(string.Format("        public I{0}Model Get{1}() => {1};",
                    //type.MakeName(), propName));
                    //sbModelInterface.AppendFormat("        I{0}Model Get{1}();", type.MakeName(), propName).AppendLine();
                    sbImplicitConvertionModels.Add($"                {colName} = obj.{colName}");
                    sb.AppendLine();
                  }
                }
                else if (columns.Any(x => x.Id == rel.Rel.MasterColumnId))
                {
                  // var type = Functions.Singularize (rel.Foreign.Name);
                  // sb.AppendFormat (@"    [ForeignKey(nameof({0}))]", colName).AppendLine ();
                  // sb.AppendFormat ("    public ICollection<{0}> {1} {{ get; set; }}", type.MakeName (), rel.Master.Name.MakeName ()).AppendLine ();
                }
                else
                {
                  var attrib = string.Format(@"    [ForeignKey(nameof({0}))]", colName);
                  if (!sb.ToString().Contains(attrib))
                  {//if (propName == "PettyCashCreditGl")
                   //    propName = propName;
                    importGroups.Add(dbGroups.FirstOrDefault(x => x.TableId == rel.Master.Id)?.GroupName);
                    var type = Functions.Singularize(rel.Master.Name);
                    sb.AppendFormat(@"    [ForeignKey(nameof({0}))]", colName).AppendLine();
                    sb.AppendFormat("    public {0}? {1} {{ get; set; }}", type.MakeName(), propName).AppendLine();
                    sbModelInterface.AppendFormat("        string? {1}_Caption {{ get; set;}}",
type.MakeName(), propName).AppendLine();
                    sb.AppendFormat(@" //[Reactive, NotMapped]
        public string? {1}_Caption {{ get; set; }}
", type.MakeName(), propName).AppendLine();
                    sb.Append(string.Format("        public I{0}Model? Get{1}() => {1};",
                    type.MakeName(), propName)).AppendLine();
                    sbModelInterface.AppendFormat("        I{0}Model? Get{1}();", type.MakeName(), propName).AppendLine();
                    sb.AppendLine();

                  }
                }
              }
            }
            else
            {
              var small = $"_{Functions.MakeFirstSmallOtherLetterCapital(colName, false)}";
              var prop = string.Format(@"        public {1} {0} {{ get; init; }}
", colName, column.Type);
              sb.Append(prop).AppendLine();
              sbModelInterface.Append(string.Format(@"        {2} {0} {{ get; set; }}", colName, small, column.Type)).AppendLine();
             sbModelDTO.AppendFormat(@"        public {2} {0} {{ get; set; }}", colName, small, column.Type).AppendLine();
          }
          }

          //var mRels = 

          //var mRels = from rel in allRels
          //            from rel1 in allRels
          //            where rel.Rel.MasterColumnId == rel1.Rel.ForeignColumnId
          //            && columns.Any(x => x.Id == rel1.Rel.ForeignColumnId)
          //    select rel;
          var allRelations = allRels.Where(x => /*columns.Any(y => y.IsUnique && y.Id != x.Rel.ForeignColumnId) ||*/ columns.Any(y => y.Id == x.Rel.MasterColumnId)).ToArray();
          if (allRelations.Any())
            sbForeign.AppendFormat(@" Triggers <{0}>.Updating += entry =>
            {{
                if (entry.Original.Id != entry.Entity.Id)
                {{
                    var db = entry.Context as DataContext;
                    
", singular);

          var relCols = allRelations.GroupBy(x => x.Master.Id)
              .SelectMany(rels =>
              {
                var str = new List<string>();
                foreach (var rel in rels)
                {
                  var isMaster = dbColumns.Any(c => (c.ColumnName == rel.ForeignColumn) && c.IsUnique);

                  if (isMaster)
                    str = str.ToList();

                  var propName = rel.Foreign.Name;
                  var lss = rels.ToArray();
                  System.Console.WriteLine("Count " + rel.Master.Name + " " + lss.Count());
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

                  //if (!isMaster && !propName.EndsWith("s"))
                  //{
                  //    propName += "s";
                  //}

                  var xNameUpper = Functions.MakeAllFirstLetterCapital(rel.Foreign.Name.MakeName(), false);
                  var xNameLower = Functions.MakeFirstSmallOtherLetterCapital(xNameUpper);

                  var importGroup = dbGroups.FirstOrDefault(x => x.TableId == rel.Foreign.Id)?.GroupName;
                  importGroups.Add(importGroup);
                  var type = Functions.Singularize(rel.Foreign.Name);

                  if (isMaster)
                  {
                    propName = Functions.Singularize(propName) ?? propName;

                    str.Add(string.Format("    public {0} {1} {{ get; set; }}",
                                    type.MakeName(), propName));
                    str.Add(string.Format("        public I{0}Model? Get{1}() => {1};\r\n",
                        type.MakeName(), propName));
                    //public IUserLanguageTranslationsTypeModel GetDefaultUserTranslationType() => DefaultUserTranslationType;
                    sbModelInterface.AppendFormat("        I{0}Model? Get{1}();", type.MakeName(), propName).AppendLine();
                    sbModelInterface.AppendFormat(@"        string? {1}_Caption {{ get; set; }}", type.MakeName(), propName).AppendLine();
                    str.Add(string.Format(@" //[Reactive, NotMapped]
        public string? {1}_Caption {{ get; set; }}
", type.MakeName(), propName));
                  }
                  else str.Add(string.Format("    public ICollection<{0}>? {1} {{ get; set; }}",
                              type.MakeName(), rel.Foreign.Name.MakeName()));
                  str.Add(string.Format("        public I{0}Model[]? Get{1}() => {1}.ToArray();",
                         type.MakeName(), rel.Foreign.Name.MakeName()));
                  sbModelInterface.AppendFormat("        I{0}Model[]? Get{1}();", type.MakeName(), rel.Foreign.Name.MakeName()).AppendLine();

                  sbForeign.AppendFormat(@"
                    var {0} = db.{1}.Where(x => x.{2} == entry.Original.Id);
                    foreach (var {3} in {0})
                        {3}.{2} = entry.Entity.Id; 
                        ", xNameLower, xNameUpper, rel.ForeignColumn.MakeName(), Functions.Singularize(xNameLower));

                  var relPropName = rel.ForeignColumn;
                  if (Regex.IsMatch(relPropName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
                  {
                    var len = Regex.Match(relPropName,
                                        "(id|code|no|number|key) *$",
                                        RegexOptions.IgnoreCase).Value.Length;
                    relPropName = relPropName.Trim().Substring(0, relPropName.Length - len);
                    relPropName = relPropName.MakeName();
                  }

                  var builder = string.Format(@"  builder.Entity<{0}>()
    .Property(t => t.Id) 
    .HasMaxLength(BaseModel.___PrimaryMaxLength___);", type.MakeName(), rel.ForeignColumn.MakeName());

                  if (contextConfigs.ContainsKey(group[0]))
                  {
                    contextConfigs[group[0]].Add(new Tuple<string, string>(builder, importGroup));
                  }
                  else
                  {
                    contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, importGroup) });
                  }
                  propName = rel.Foreign.Name.MakeName().MakeName();
                  builder = string.Format(@"  builder.Entity<{0}>()
          .HasMany(x => x.{1})
          .WithOne(u => u.{2})
          .HasForeignKey(u => u.{3})
          .OnDelete(DeleteBehavior.Restrict);", singular, propName, relPropName, rel.ForeignColumn.MakeName());

                  //                       builder = string.Format(@"  builder.Entity<{0}>()
                  //.HasMany(x => x.{1})
                  //.WithOne(u => u.{2})
                  //.HasForeignKey(u => u.{3})
                  //.OnDelete(DeleteBehavior.Restrict);", singular, propName, relPropName, rel.ForeignColumn.MakeName());

                  if (contextConfigs.ContainsKey(group[0]))
                  {
                    contextConfigs[group[0]].Add(new Tuple<string, string>(builder, importGroup));
                  }
                  else
                  {
                    contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, importGroup) });
                  }
                }
                return str;
              }).Distinct().ToArray();

          importGroups = importGroups.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
          if (importGroups.Any())
          {
            var mmImports = importGroups.Select(x =>
            {
              var grp = x.Replace("/", "\\").Split('\\')
                              .Select(x => x.MakeName())
                              .ToArray();

              var startIndex = 0;
              var broken = false;
              for (int i = 0; i < group.Length; i++)
              {
                startIndex = i;
                if (grp.Length > i)
                {
                  if (grp[i] != group[i])
                  {
                    broken = true;
                    break;
                  }
                }
              }
              if (!broken && grp.Length <= group.Length)
                return null;
              return string.Join(".", grp);
            }).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray();
            sb.Insert(0, string.Join(";\r\nusing KFA.DynamicsAssistant.Infrastructure.Data.", mmImports) + ";\r\n").Insert(0, "using KFA.DynamicsAssistant.Infrastructure.Data");
          }

          foreach (var rel in relCols)
          {
            sb.AppendLine(rel);
          }

          var tt = getImplicitConversionCaptions(table, allRels.Select(x => x.Rel));
          if (tt?.Length > 4)
            tt = $",\r\n{tt}";

         sbModelDTO.AppendLine($@"public override dynamic? ToModel()
  {{
    return ({singular})this;
  }}
}}");
          //sb.AppendLine("}");

          var path = Path.Combine(Defaults.MainPath, "Models", string.Join("/", group));
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        StringBuilder sbEquator = new();
        foreach (var col in columns)
          if (!col.IsPrimary)
          {
            //sbModelDTO.AppendFormat(@"  public {0}? {1} {{ get; set; }}", col.Type, col.StrimLinedName?.MakeName());
            //sbModelDTO.AppendLine();

            //if(sbEquator.Length > 3)
            //  sbEquator.Append(", ");

            sbEquator.AppendFormat(@"      {0} = obj.{1},
", col.StrimLinedName?.MakeName(), col.StrimLinedName?.MakeName());
            // sbEquator.AppendLine();
          }

        sb.AppendFormat(@"
  public static implicit operator {0}DTO({1} obj)
  {{
    return new {2}DTO
    {{
", singular, singular, singular);
        sb.Append(sbEquator);

        sb.AppendLine($@"      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    }};
  }}
  public static implicit operator {singular}({singular}DTO obj)
  {{
    return new {singular}
    {{");
        sb.Append(sbEquator);

        sb.Append($@"Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    }};
  }}

  public override BaseDTO<T>? ToDTO<T>()
  {{
    dynamic obj = ({singular}DTO)this;
    return obj as BaseDTO<T>;
  }}
}}");


        path = Path.Combine(path, string.Format("{0}.cs", singular));
          File.WriteAllText(path, sb.ToString());


          var dtoPath = Path.Combine(Defaults.MainPath, "NewDTOModels");
          if (!Directory.Exists(dtoPath))
            Directory.CreateDirectory(dtoPath);
          dtoPath = Path.Combine(dtoPath, string.Format("{0}DTO.cs", singular));

        path = Path.Combine(Defaults.MainPath, "ContractModels");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        path = Path.Combine(path, string.Format("I{0}Model.cs", singular));

       


        sbModelInterface.Append(@"
   }
");
          File.WriteAllText(path, sbModelInterface.ToString());
          File.WriteAllText(dtoPath, sbModelDTO.ToString());

        if (allRelations.Any())
            sbForeign.AppendFormat(@"
                }}
            }};");
          // if (tableCount > 5)
          //     break;
        }
        if (contextConfigs.Any())
        {
          var path = Path.Combine(Defaults.MainPath, "EntityConfigs");
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

          var body = @"using Microsoft.EntityFrameworkCore;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models.{1};

namespace Pilgrims.Projects.Assistant.DataLayer.Data.EntityConfigs
{{
    public static class {2}
    {{
        internal static void OnModelCreating (ModelBuilder builder) {{
      
{0}
               
    }}
    }}
}}";

          foreach (var item in contextConfigs)
          {
            try
            {
              var name = item.Key.MakeName() + "EfConfig";
              var text = string.Join("\r\n\r\n", item.Value.Select(x => x.Item1).Distinct());
              var usingTexts = item.Value.Select(x => x.Item2?.Replace("/", "\\")).Distinct();

              var usingText = string.Join(";\r\nusing using Pilgrims.Projects.Assistant.DataLayer.Data.Models",
                  usingTexts.Select(x => string.Join(".", x.Split('\\')
                     .Select(x => x?.MakeName()))));

              var txt = string.Format(body, text, usingText, name);
              File.WriteAllText(Path.Combine(path, name + ".cs"), txt);
            }
            catch (Exception ex)
            {
              System.Console.WriteLine(ex);
            }
          }
        }

        {
          var path = Path.Combine(Defaults.MainPath, "EntityConfigs");
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          var cfg = @"using System.Linq;
using EntityFrameworkCore.Triggers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models.System.UserManagement;
using PPMS.Console.Data;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.src.EntityConfigs
{{
    static class ForeignCheck
    {{
        internal static void InstallForeignChecks()
        {{
            {0}
        }}
    }}
}}";

          File.WriteAllText(Path.Combine(Defaults.MainPath, "ForeignCheck.cs"), string.Format(cfg, sbForeign.ToString()));
        }

        var foreigns = allRels
            .GroupBy(x => x.Foreign.Name)
            .OrderBy(x => x.Key)
            .Select(x => $@"            builder.Entity<{Functions.Singularize(x.Key).MakeName()}>()
                .HasIndex(p => new {{ {string.Join(", ", x.Select(y => $"p.{y.ForeignColumn.MakeName()}"))} }}).IsUnique();");

        var dbTables = tables.Select(x => x.Name).ToList();
        var ss = $@"namespace Pilgrims.Projects.Assistant.Contracts.Classes
{{
    public enum TablesEnum
    {{
{string.Join(",\r\n", dbTables.Select(x => $"        {x.MakeName()} = {dbTables.IndexOf(x) + 1}"))}
    }}
}}";
        File.WriteAllText(Path.Combine(Defaults.MainPath, "TablesEnum.cs"), ss);
        if (foreigns.Any())
        {
          var path = Path.Combine(Defaults.MainPath, "EntityConfigs");
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

          var text = $@"using Microsoft.EntityFrameworkCore;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.Configs
{{
    static class AdditionalChecks
    {{
        internal static void Check(ref ModelBuilder builder)
        {{
{string.Join("\r\n", foreigns)}
        }}
    }}
}}";
          path = Path.Combine(path, "AdditionalChecks.cs");
          File.WriteAllText(path, text);
        }
        var tbls = string.Join("\r\n", tables.ToArray().Select(c => $@"
        [Fact]
        public async Task TestModelCreation_I{c.Name.Singularize().MakeName()}Model()
        {{          
            Assert.NotNull(container.Resolve<I{c.Name.Singularize().MakeName()}Model>>());
        }}
"));

        var textFile = testClasses.Replace("<<<1>>>", tbls);
        var pathe = Path.Combine(Defaults.MainPath, "Tests");
        if (!Directory.Exists(pathe))
          Directory.CreateDirectory(pathe);

        File.WriteAllText(pathe, textFile);
      }
      catch (Exception exception)
      {
        System.Console.WriteLine("Error: " + exception.ToString());
      }
    }


    //        internal static void GenerateModels()
    //        {
    //            try
    //            {
    //                using var db = new Data.Context();
    //                var tables = db.Tables;
    //                var allRels = db.Relations.ToArray()
    //                    .Distinct(new RelationComparer())
    //                    .Where(x => x != null)
    //                    .Select(x =>
    //                    {

    //                        DatabaseTable master = null, foreign = null;
    //                        try
    //                        {
    //                            foreign = db.Tables.FirstOrDefault(y => y.Id == db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId).TableId);
    //                        }
    //                        catch { }
    //                        try
    //                        {
    //                            master = db.Tables.FirstOrDefault(y => y.Id == db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId).TableId);
    //                        }
    //                        catch { }

    //                        return new
    //                        {
    //                            Rel = x,
    //                            Master = master,
    //                            Foreign = foreign,
    //                            MasterColumn = db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId)?.ColumnName,
    //                            ForeignColumn = db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)?.ColumnName,
    //                            MasterTableColumn = db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId),
    //                            ForeignTableColumn = db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)
    //                        };
    //                    }).Where(x => x.Master != null && x.Foreign != null).ToArray();
    //                var dbGroups = db.Groups.ToArray();
    //                var dbColumns = db.Columns.ToArray();

    //                var contextConfigs = new Dictionary<string, List<Tuple<string, string>>>();
    //                //int tableCount = 1;
    //                var sbForeign = new StringBuilder();


    //                var relColIds = allRels.SelectMany(x => new[] { x.Rel.ForeignColumnId, x.Rel.MasterColumnId }).Distinct().ToArray();

    //                foreach (var table in tables)
    //                {
    //                    //System.Console.WriteLine("Processing table " + table.Name + " => " + tableCount++ + " / " + tables.Count());
    //                    var groupName = "General";
    //                    var columns = dbColumns.Where(x => x.TableId == table.Id).ToArray();
    //                    var groupObj = dbGroups.FirstOrDefault(x => x.TableId == table.Id);
    //                    if (groupObj != null &&
    //                        !string.IsNullOrWhiteSpace(groupObj.GroupName))
    //                        groupName = groupObj.GroupName;

    //                    var importGroups = new List<string>();

    //                    var group = groupName.Replace("/", "\\").Split('\\')
    //                        .Select(x => Functions.MakeAllFirstLetterCapital(x, false).Replace(" ", ""))
    //                        .ToArray();

    //                    var name = Functions.MakeAllFirstLetterCapital(table.StrimLinedName, false);
    //                    var singular = Functions.Singularize(name)?.Replace(" ", "");
    //                    var tblName = $"tbl_{name?.Replace(" ", "_")?.ToLower()}";

    //                    var sb = new StringBuilder();
    //                    var sbMainModel = new StringBuilder();
    //                    var sbModelInterface = new StringBuilder(@"using System;
    //using System.Collections.Generic;
    //using System.Text;
    //using System.Threading.Tasks;

    //namespace Pilgrims.Projects.Assistant.Contracts.DataLayer.Models
    //{
    //    public interface ICostCentre : IBaseModel
    //    {
    //");
    //                    var sbImplicitConvertionModels = new List<String>();
    //                    var sbImplicitConvertionMasterMethods = new List<String>();

    //                    sbMainModel.AppendFormat(@"using ReactiveUI;
    //using System;

    //namespace Pilgrims.Projects.Assistant.Contracts.Data.DataModels
    //{{
    //    [MessagePack.MessagePackObject]
    //    public sealed class {1}QueryModel : BaseQueryModel
    //    {{  
    //        [MessagePack.IgnoreMember]
    //        public override string ___tableName___ {{ get; protected set; }} = ""{0}"";
    //        [MessagePack.IgnoreMember]
    //        public override object QueryModel => new {{ Id{2} }};
    //",
    //                       table.StrimLinedName, singular, columns.Count() > 1 ? ", " + string.Join(", ", columns.Where(x => !x.IsPrimary).Select(x => Functions.MakeAllFirstLetterCapital(x.StrimLinedName, false)
    //                                   .Replace(" ", ""))) : "")
    //                       .AppendLine();


    //                    sb.AppendFormat(@"using System;
    //using System.Collections.Generic;
    //using System.ComponentModel.DataAnnotations;
    //using System.ComponentModel.DataAnnotations.Schema;
    //using ReactiveUI;
    //using Pilgrims.Projects.Assistant.DataLayer.Data;

    //namespace Pilgrims.Projects.Assistant.DataLayer.Data.Models {{
    //  [Table (""{0}"")]
    //  internal class {1}: BaseModel {{
    //    public override string ___tableName___ {{ get; protected set; }} = ""{0}"";",
    //                            tblName, singular, string.Join(".", group))
    //                        .AppendLine();

    //                    int messagePackCount = 4;
    //                    foreach (var column in columns)
    //                    {
    //                        var rels = allRels.Where(x => x.Rel.MasterColumnId == column.Id || x.Rel.ForeignColumnId == column.Id).ToArray();
    //                        var colName = Functions.MakeAllFirstLetterCapital(column.StrimLinedName, false)
    //                            .Replace(" ", "");

    //                        if (!column.IsPrimary)
    //                            sbImplicitConvertionModels.Add($"                {colName} = obj.{colName}");

    //                        if (column.IsUnique)
    //                        {
    //                            string builder;
    //                            if (column.IsPrimary)
    //                                builder = string.Format(@"  builder.Entity<{0}>()
    //        .HasIndex(b => b.Id)
    //        .IsUnique();", singular, colName);
    //                            else
    //                                builder = string.Format(@"  builder.Entity<{0}>()
    //        .HasIndex(b => b.{1})
    //        .IsUnique();", singular, colName);

    //                            if (contextConfigs.ContainsKey(group[0]))
    //                            {
    //                                contextConfigs[group[0]].Add(new Tuple<string, string>(builder, groupName));
    //                            }
    //                            else
    //                            {
    //                                contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, groupName) });
    //                            }
    //                        }

    //                        if (!column.IsNullable)
    //                            sb.Append("    [Required]").AppendLine();
    //                        if (column.IsUnique)
    //                            sb.Append("//    [Index(IsUnique = true)]").AppendLine();


    //                        //          if (column.Type.ToLower().Contains("double")
    //                        //              || column.Type.ToLower().Contains("decimal"))
    //                        //          {

    //                        //              var builder = string.Format(@" builder.Entity<{0}>()
    //                        //.Property(t => t.{1})
    //                        //.HasPrecision(20,5);", singular, colName);

    //                        //              if (contextConfigs.ContainsKey(group[0]))
    //                        //              {
    //                        //                  contextConfigs[group[0]].Add(new Tuple<string, string>(builder, groupName));
    //                        //              }
    //                        //              else
    //                        //              {
    //                        //                  contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, groupName) });
    //                        //              }
    //                        //          }


    //                        if (!column.IsPrimary && relColIds.Contains(column.Id))
    //                        {

    //                            var builder = string.Format(@" builder.Entity<{0}>()
    //              .Property(t => t.{1})
    //              .HasMaxLength(BaseModel.___PrimaryMaxLength___);", singular, colName);

    //                            if (contextConfigs.ContainsKey(group[0]))
    //                            {
    //                                contextConfigs[group[0]].Insert(0, new Tuple<string, string>(builder, groupName));
    //                            }
    //                            else
    //                            {
    //                                contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, groupName) });
    //                            }
    //                        }
    //                        else if (!column.IsPrimary)
    //                        {
    //                            if (column.Length > 3 && column.Length <20000)
    //                                sb.AppendFormat(@"    [MaxLength({0}, ErrorMessage=""Please {1} must be {0} characters or less"")]", column.Length, column.ColumnName.ToLower()).AppendLine();
    //                        }

    //                        sb.AppendFormat(@"    [Column (""{0}"")]", column.ColumnName.Replace(" ", "_").ToLower()).AppendLine();
    //                        if (column.IsPrimary)
    //                        {
    //                            var prop = @"    public override string Id 
    //        {
    //            get => id;
    //            protected set
    //            {
    //                var objValue = ValidateProperty(value, nameof(Id), this);
    //                if (id == objValue)
    //                    return;
    //                this.RaiseAndSetIfChanged(ref id, objValue, nameof(Id));
    //            }
    //        }
    //        private string id;
    //";
    //                            sb.AppendLine(prop);
    //                            sbMainModel.AppendLine("[MessagePack.Key(0)]");
    //                            sbMainModel.AppendLine(prop.Replace("protected set", "set"));
    //                        }
    //                        else if (rels.Any())
    //                        {
    //                            var small = Functions.MakeFirstSmallOtherLetterCapital(colName, false);
    //                            var prop = string.Format(@"        public string {0}
    //        {{
    //            get => {1};
    //            set
    //            {{
    //                var objValue = ValidateProperty(value, nameof({0}), this);
    //                if ({1} == objValue)
    //                    return;
    //                this.RaiseAndSetIfChanged(ref {1}, objValue, nameof({0}));
    //            }}
    //        }}
    //        private string {1};
    //", colName, small);

    //                            sb.Append(prop).AppendLine();
    //                            sbMainModel.AppendFormat("[MessagePack.Key({0})]", messagePackCount++);
    //                            sbMainModel.Append(prop).AppendLine();

    //                            foreach (var rel in rels)
    //                            {
    //                                var propName = colName;
    //                                if (Regex.IsMatch(propName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
    //                                {
    //                                    var len = Regex.Match(propName,
    //                                              "(id|code|no|number|key) *$",
    //                                              RegexOptions.IgnoreCase).Value.Length;
    //                                    propName = propName.Trim().Substring(0, propName.Length - len);
    //                                    propName = propName.MakeName();
    //                                }

    //                                if (columns.Any(x => x.Id == rel.Rel.ForeignColumnId)
    //                                    && columns.Any(x => x.Id == rel.Rel.MasterColumnId))
    //                                {
    //                                    var attrib = string.Format(@"    [ForeignKey(nameof({0}))]", colName);
    //                                    if (!sb.ToString().Contains(attrib))
    //                                    {
    //                                        var type = Functions.Singularize(rel.Master.Name);
    //                                        sb.Append(attrib).AppendLine();
    //                                        sb.AppendFormat("    public {0} {1} {{ get; set; }}", type.MakeName(), propName).AppendLine();
    //                                        sbMainModel.AppendFormat("[MessagePack.Key({0})]", messagePackCount++);
    //                                        //sbImplicitConvertionMasterMethods.Add("                {0}_Caption = {0}?.TableName,", type.MakeName(), propName).AppendLine();
    //                                        sbMainModel.AppendFormat("    public string {1}_Caption {{ get; set; }}", type.MakeName(), propName).AppendLine();
    //                                        sbImplicitConvertionModels.Add($"                {colName} = obj.{colName}");
    //                                        sb.AppendLine();
    //                                    }
    //                                }
    //                                else if (columns.Any(x => x.Id == rel.Rel.MasterColumnId))
    //                                {
    //                                    // var type = Functions.Singularize (rel.Foreign.Name);
    //                                    // sb.AppendFormat (@"    [ForeignKey(nameof({0}))]", colName).AppendLine ();
    //                                    // sb.AppendFormat ("    public ICollection<{0}> {1} {{ get; set; }}", type.MakeName (), rel.Master.Name.MakeName ()).AppendLine ();
    //                                }
    //                                else
    //                                {
    //                                    var attrib = string.Format(@"    [ForeignKey(nameof({0}))]", colName);
    //                                    if (!sb.ToString().Contains(attrib))
    //                                    {//if (propName == "PettyCashCreditGl")
    //                                     //    propName = propName;
    //                                        importGroups.Add(dbGroups.FirstOrDefault(x => x.TableId == rel.Master.Id)?.GroupName);
    //                                        var type = Functions.Singularize(rel.Master.Name);
    //                                        sb.AppendFormat(@"    [ForeignKey(nameof({0}))]", colName).AppendLine();
    //                                        sb.AppendFormat("    public {0} {1} {{ get; set; }}", type.MakeName(), propName).AppendLine();
    //                                        sbMainModel.AppendFormat("[MessagePack.Key({0})]", messagePackCount++);
    //                                        //sbImplicitConvertionMasterMethods.Add("                {0}_Caption = {0}?.TableName,", type.MakeName(), propName).AppendLine();
    //                                        sbMainModel.AppendFormat("    public string {1}_Caption {{ get; set;}}", type.MakeName(), propName).AppendLine();
    //                                        sb.AppendLine();

    //                                    }
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            var small = Functions.MakeFirstSmallOtherLetterCapital(colName, false);
    //                            var prop = string.Format(@"        public {2} {0}
    //        {{
    //            get => {1};
    //            set
    //            {{
    //                var objValue = ValidateProperty(value, nameof({0}), this);
    //                if ({1} == objValue)
    //                    return;
    //                this.RaiseAndSetIfChanged(ref {1}, objValue, nameof({0}));
    //            }}
    //        }}
    //        private {2} {1};
    //", colName, small, column.Type);
    //                            sb.Append(prop).AppendLine();
    //                            sbMainModel.AppendFormat("[MessagePack.Key({0})]", messagePackCount++);
    //                            sbMainModel.Append(prop).AppendLine();
    //                        }
    //                    }

    //                    //var mRels = 

    //                    //var mRels = from rel in allRels
    //                    //            from rel1 in allRels
    //                    //            where rel.Rel.MasterColumnId == rel1.Rel.ForeignColumnId
    //                    //            && columns.Any(x => x.Id == rel1.Rel.ForeignColumnId)
    //                    //    select rel;
    //                    var allRelations = allRels.Where(x => /*columns.Any(y => y.IsUnique && y.Id != x.Rel.ForeignColumnId) ||*/ columns.Any(y => y.Id == x.Rel.MasterColumnId)).ToArray();
    //                    if (allRelations.Any())
    //                        sbForeign.AppendFormat(@" Triggers<{0}>.Updating += entry =>
    //            {{
    //                if (entry.Original.Id != entry.Entity.Id)
    //                {{
    //                    var db = entry.Context as DataContext;

    //", singular);

    //                    var relCols = allRelations.GroupBy(x => x.Master.Id)
    //                        .SelectMany(rels =>
    //                        {
    //                            var str = new List<string>();
    //                            foreach (var rel in rels)
    //                            {
    //                                var isMaster = dbColumns.Any(c => (c.ColumnName == rel.ForeignColumn) && c.IsUnique);

    //                                if (isMaster)
    //                                    str = str.ToList();

    //                                var propName = rel.Foreign.Name;
    //                                var lss = rels.ToArray();
    //                                System.Console.WriteLine("Count " + rel.Master.Name + " " + lss.Count());
    //                                if (rels.Count() > 1 && rel.ForeignColumn != rel.MasterColumn)
    //                                {
    //                                    propName = rel.ForeignColumn;
    //                                    if (Regex.IsMatch(propName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
    //                                    {
    //                                        var len = Regex.Match(propName,
    //                                            "(id|code|no|number|key) *$",
    //                                            RegexOptions.IgnoreCase).Value.Length;

    //                                        propName = propName.Trim().Substring(0, propName.Length - len);
    //                                    }
    //                                }
    //                                propName = propName.MakeName();

    //                                //if (!isMaster && !propName.EndsWith("s"))
    //                                //{
    //                                //    propName += "s";
    //                                //}

    //                                var xNameUpper = Functions.MakeAllFirstLetterCapital(rel.Foreign.Name.MakeName(), false);
    //                                var xNameLower = Functions.MakeFirstSmallOtherLetterCapital(xNameUpper);

    //                                var importGroup = dbGroups.FirstOrDefault(x => x.TableId == rel.Foreign.Id)?.GroupName;
    //                                importGroups.Add(importGroup);
    //                                var type = Functions.Singularize(rel.Foreign.Name);

    //                                if (isMaster)
    //                                {
    //                                    propName = Functions.Singularize(propName) ?? propName;

    //                                    str.Add(string.Format("    public {0} {1} {{ get; set; }}",
    //                                          type.MakeName(), propName));
    //                                    sbMainModel.AppendFormat("[MessagePack.Key({0})]", messagePackCount++);
    //                                    sbMainModel.AppendFormat("    public string {1}_Caption {{ get; set; }}", type.MakeName(), propName).AppendLine();
    //                                }
    //                                else str.Add(string.Format("    public ICollection<{0}> {1} {{ get; set; }}",
    //                                  type.MakeName(), rel.Foreign.Name.MakeName()));

    //                                sbForeign.AppendFormat(@"
    //                    var {0} = db.{1}.Where(x => x.{2} == entry.Original.Id);
    //                    foreach (var {3} in {0})
    //                        {3}.{2} = entry.Entity.Id; 
    //                        ", xNameLower, xNameUpper, rel.ForeignColumn.MakeName(), Functions.Singularize(xNameLower));

    //                                var relPropName = rel.ForeignColumn;
    //                                if (Regex.IsMatch(relPropName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
    //                                {
    //                                    var len = Regex.Match(relPropName,
    //                                              "(id|code|no|number|key) *$",
    //                                              RegexOptions.IgnoreCase).Value.Length;
    //                                    relPropName = relPropName.Trim().Substring(0, relPropName.Length - len);
    //                                    relPropName = relPropName.MakeName();
    //                                }

    //                                var builder = string.Format(@"  builder.Entity<{0}>()
    //    .Property(t => t.Id) 
    //    .HasMaxLength(BaseModel.___PrimaryMaxLength___);", type.MakeName(), rel.ForeignColumn.MakeName());

    //                                if (contextConfigs.ContainsKey(group[0]))
    //                                {
    //                                    contextConfigs[group[0]].Add(new Tuple<string, string>(builder, importGroup));
    //                                }
    //                                else
    //                                {
    //                                    contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, importGroup) });
    //                                }
    //                                propName = rel.Foreign.Name.MakeName().MakeName();
    //                                builder = string.Format(@"  builder.Entity<{0}>()
    //          .HasMany(x => x.{1})
    //          .WithOne(u => u.{2})
    //          .HasForeignKey(u => u.{3})
    //          .OnDelete(DeleteBehavior.Restrict);", singular, propName, relPropName, rel.ForeignColumn.MakeName());

    //                                //                       builder = string.Format(@"  builder.Entity<{0}>()
    //                                //.HasMany(x => x.{1})
    //                                //.WithOne(u => u.{2})
    //                                //.HasForeignKey(u => u.{3})
    //                                //.OnDelete(DeleteBehavior.Restrict);", singular, propName, relPropName, rel.ForeignColumn.MakeName());

    //                                if (contextConfigs.ContainsKey(group[0]))
    //                                {
    //                                    contextConfigs[group[0]].Add(new Tuple<string, string>(builder, importGroup));
    //                                }
    //                                else
    //                                {
    //                                    contextConfigs.Add(group[0], new List<Tuple<string, string>> { new Tuple<string, string>(builder, importGroup) });
    //                                }
    //                            }
    //                            return str;
    //                        }).Distinct().ToArray();

    //                    importGroups = importGroups.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
    //                    if (importGroups.Any())
    //                    {
    //                        var mmImports = importGroups.Select(x =>
    //                        {
    //                            var grp = x.Replace("/", "\\").Split('\\')
    //                                .Select(x => x.MakeName())
    //                                .ToArray();

    //                            var startIndex = 0;
    //                            var broken = false;
    //                            for (int i = 0; i <group.Length; i++)
    //                            {
    //                                startIndex = i;
    //                                if (grp.Length > i)
    //                                {
    //                                    if (grp[i] != group[i])
    //                                    {
    //                                        broken = true;
    //                                        break;
    //                                    }
    //                                }
    //                            }
    //                            if (!broken && grp.Length <= group.Length)
    //                                return null;
    //                            return string.Join(".", grp);
    //                        }).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray();
    //                        sb.Insert(0, string.Join(";\r\nusing Pilgrims.Projects.Assistant.DataLayer.Data.Models.", mmImports) + ";\r\n").Insert(0, "using Pilgrims.Projects.Assistant.DataLayer.Data.Models.");
    //                    }

    //                    foreach (var rel in relCols)
    //                    {
    //                        sb.AppendLine(rel);
    //                    }

    //                    var tt = getImplicitConversionCaptions(table, allRels.Select(x => x.Rel));
    //                    if (tt?.Length > 4)
    //                        tt = $",\r\n{tt}";

    //                    sb.Append($@"

    //        public static implicit operator {singular}(Contracts.Data.DataModels.{singular}QueryModel obj)
    //        {{
    //            return new {singular}
    //            {{
    //                DateInserted = obj.DateInserted,
    //                DateUpdated = obj.DateUpdated,
    //                OriginatorId = obj.OriginatorId,
    //                SetId = obj.Id,
    //                RecordCommentId = obj.RecordCommentId,
    //                RecordVerificationId = obj.RecordVerificationId,
    //{string.Join(",\r\n", sbImplicitConvertionModels)}
    //            }};
    //        }}

    //        public static implicit operator Contracts.Data.DataModels.{singular}QueryModel({singular} obj)
    //        {{
    //            return new Contracts.Data.DataModels.{singular}QueryModel
    //            {{
    //                DateInserted = obj.DateInserted,
    //                DateUpdated = obj.DateUpdated,
    //                OriginatorId = obj.OriginatorId,
    //                SetId = obj.Id,
    //                RecordCommentId = obj.RecordCommentId,
    //                RecordVerificationId = obj.RecordVerificationId,
    //{string.Join(",\r\n", sbImplicitConvertionModels)}{tt}
    //            }};
    //        }}
    //");



    //                    sb.AppendLine("    }");
    //                    sb.AppendLine("}");

    //                    var path = Path.Combine(Defaults.MainPath, "Models", string.Join("/", group));
    //                    if (!Directory.Exists(path))
    //                        Directory.CreateDirectory(path);

    //                    path = Path.Combine(path, string.Format("{0}.cs", singular));
    //                    //sb = sb.Replace("public Autoincrement ", "public General.Autoincrement ");
    //                    //sb = sb.Replace("<ProjectOption>", "<ProjectOption.ProjectOptions.ProjectOption>");
    //                    //// sb = sb.Replace ("using Pilgrims.Projects.Assistant.DataLayer.Data.Models.", "");
    //                    //sb = sb.Replace("public DataType ", "public Settings.Project.DataSettings.DataType ");
    //                    //sb = sb.Replace("public Project ", "public System.General.Project ");
    //                    File.WriteAllText(path, sb.ToString());


    //                    path = Path.Combine(Defaults.MainPath, "ContractModels");
    //                    if (!Directory.Exists(path))
    //                        Directory.CreateDirectory(path);
    //                    path = Path.Combine(path, string.Format("{0}QueryModel.cs", singular));
    //                    sbMainModel.Append(@"
    //   }
    //}");
    //                    File.WriteAllText(path, sbMainModel.ToString());

    //                    if (allRelations.Any())
    //                        sbForeign.AppendFormat(@"
    //                }}
    //            }};");
    //                    // if (tableCount > 5)
    //                    //     break;
    //                }
    //                if (contextConfigs.Any())
    //                {
    //                    var path = Path.Combine(Defaults.MainPath, "EntityConfigs");
    //                    if (!Directory.Exists(path))
    //                        Directory.CreateDirectory(path);

    //                    var body = @"using Microsoft.EntityFrameworkCore;
    //using Pilgrims.Projects.Assistant.DataLayer.Data.Models.{1};

    //namespace Pilgrims.Projects.Assistant.DataLayer.Data.EntityConfigs
    //{{
    //    public static class {2}
    //    {{
    //        internal static void OnModelCreating (ModelBuilder builder) {{

    //{0}

    //    }}
    //    }}
    //}}";

    //                    foreach (var item in contextConfigs)
    //                    {
    //                        try
    //                        {
    //                            var name = item.Key.MakeName() + "EfConfig";
    //                            var text = string.Join("\r\n\r\n", item.Value.Select(x => x.Item1).Distinct());
    //                            var usingTexts = item.Value.Select(x => x.Item2?.Replace("/", "\\")).Distinct();

    //                            var usingText = string.Join(";\r\nusing using Pilgrims.Projects.Assistant.DataLayer.Data.Models",
    //                                usingTexts.Select(x => string.Join(".", x.Split('\\')
    //                                   .Select(x => x?.MakeName()))));

    //                            var txt = string.Format(body, text, usingText, name);
    //                            File.WriteAllText(Path.Combine(path, name + ".cs"), txt);
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            System.Console.WriteLine(ex);
    //                        }
    //                    }
    //                }

    //                {
    //                    var path = Path.Combine(Defaults.MainPath, "EntityConfigs");
    //                    if (!Directory.Exists(path))
    //                        Directory.CreateDirectory(path);
    //                    var cfg = @"using System.Linq;
    //using EntityFrameworkCore.Triggers;
    //using Pilgrims.Projects.Assistant.DataLayer.Data.Models.System.UserManagement;
    //using PPMS.Console.Data;

    //namespace Pilgrims.Projects.Assistant.DataLayer.Data.src.EntityConfigs
    //{{
    //    static class ForeignCheck
    //    {{
    //        internal static void InstallForeignChecks()
    //        {{
    //            {0}
    //        }}
    //    }}
    //}}";

    //                    File.WriteAllText(Path.Combine(Defaults.MainPath, "ForeignCheck.cs"), string.Format(cfg, sbForeign.ToString()));
    //                }

    //                var foreigns = allRels
    //                    .GroupBy(x => x.Foreign.Name)
    //                    .OrderBy(x => x.Key)
    //                    .Select(x => $@"            builder.Entity<{Functions.Singularize(x.Key).MakeName()}>()
    //                .HasIndex(p => new {{ {string.Join(", ", x.Select(y => $"p.{y.ForeignColumn.MakeName()}"))} }}).IsUnique();");

    //                var dbTables = tables.Select(x => x.Name).ToList();
    //                var ss = $@"namespace Pilgrims.Projects.Assistant.Contracts.Classes
    //{{
    //    public enum TablesEnum
    //    {{
    //{string.Join(",\r\n", dbTables.Select(x => $"        {x.MakeName()} = {dbTables.IndexOf(x) + 1}"))}
    //    }}
    //}}";
    //                File.WriteAllText(Path.Combine(Defaults.MainPath, "TablesEnum.cs"), ss);
    //                if (foreigns.Any())
    //                {
    //                    var path = Path.Combine(Defaults.MainPath, "EntityConfigs");
    //                    if (!Directory.Exists(path))
    //                        Directory.CreateDirectory(path);

    //                    var text = $@"using Microsoft.EntityFrameworkCore;
    //using Pilgrims.Projects.Assistant.DataLayer.Data.Models;

    //namespace Pilgrims.Projects.Assistant.DataLayer.Data.Configs
    //{{
    //    static class AdditionalChecks
    //    {{
    //        internal static void Check(ref ModelBuilder builder)
    //        {{
    //{string.Join("\r\n", foreigns)}
    //        }}
    //    }}
    //}}";
    //                    path = Path.Combine(path, "AdditionalChecks.cs");
    //                    File.WriteAllText(path, text);
    //                }
    //            }
    //            catch (Exception exception)
    //            {
    //                System.Console.WriteLine("Error: " + exception.ToString());
    //            }
    //        }

    static string getBodyOld(DatabaseTable table, IEnumerable<TableRelation> rels, bool asProjected = false)
    {
      if (asProjected)
        return $@"                    #region {table.Name}
                        if (type == typeof({table.Name.MakeName().Singularize()}))
                        {{
                          return new Repository<{table.Name.MakeName().Singularize()}>()
                                .Get(CheckQuery)
                                .ProjectTo<{table.Name.MakeName().Singularize()}QueryModel>(new MapperConfiguration(cfg => cfg.CreateMap<{table.Name.MakeName().Singularize()}, {table.Name.MakeName().Singularize()}QueryModel>(){getCaptions(table, rels)}
                           ));
                        }}
                    #endregion";

      return $@"                    #region {table.Name}
                    if (type == typeof({table.Name.MakeName().Singularize()}))
                    {{
                          return new Repository<{table.Name.MakeName().Singularize()}>()
                                .Get(CheckQuery);
                    }}
                    #endregion";
    }

    static string testClasses = @"using Pilgrims.Projects.Assistant.Contracts.DataLayer;
using Pilgrims.Projects.Assistant.Contracts.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Xunit;

namespace Pilgrims.Projects.Assistant.DataLayer.UnitTests
{
    public class ModelCreationTests
    {
        [Fact]
        public async Task Test_MainModel_Creation()
        {
            Assert.Null(container.Resolve<IDataRepository<IBaseModel>>()?.Delete(""));           
        }

<<<1>>> 
    }
}
";

    static string getBody(DatabaseTable table, IEnumerable<TableRelation> rels, bool asProjected = false)
    {
      var cols = string.Join(",\r\n", table.Columns
          .Where(x => !x.IsPrimary)
          .Select(column =>
          Functions.MakeAllFirstLetterCapital(column.StrimLinedName, false)
                      .Replace(" ", ""))
          .Select(x => $"                              {x} = obj.{x}"));

      var tt = getImplicitConversionCaptions(table, rels);
      if (tt?.Length > 4)
        cols = $"{cols},\r\n{tt.Replace("?.", ".")}";

      if (asProjected)
        return $@"                    #region {table.Name}
                        if (type == typeof({table.Name.MakeName().Singularize()}))
                        {{
                          return new Repository<{table.Name.MakeName().Singularize()}>()
                                .Get(CheckQuery)
                          .Select(obj => new {table.Name.MakeName().Singularize()}QueryModel
                          {{
                              DateInserted = obj.DateInserted,
                              DateUpdated = obj.DateUpdated,
                              OriginatorId = obj.OriginatorId,
                              SetId = obj.Id,
                              RecordCommentId = obj.RecordCommentId,
                              RecordVerificationId = obj.RecordVerificationId,
{cols}
                          }});
                     }}
                #endregion
       ";

      return $@"                    #region {table.Name}
                        if (type == typeof({table.Name.MakeName().Singularize()}))
                          return new Repository<{table.Name.MakeName().Singularize()}>()
                                .Get(CheckQuery);
                    #endregion
       ";
    }

    static string getCaptions(DatabaseTable table, IEnumerable<TableRelation> rels)
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

    static string getImplicitConversionCaptions(DatabaseTable table, IEnumerable<TableRelation> rels)
    {
      return string.Join(",\r\n", rels.Where(x => table.Columns
      .Select(y => y.Id).Contains(x.ForeignColumnId))
      .Select(x =>
      {
        var propName = x.ForeignColumn.ColumnName;
        var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
        if (reg.Success)
          propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
        propName = propName.MakeName();

        var tt = string.Join(@" + "" - "" + ", Templates.GetTableMasters(x.MasterColumn.Table)
              .Select(d => $@"obj.{propName}?.{(d.IsPrimary ? "Id" : d.ColumnName.MakeName())}"));
        return $@"                {propName}_Caption = {tt}";
      }).Distinct());
    }

    internal static void GenerateEfQueries()
    {
      using var db = new Data.Context();
      var tables = db
          .Tables.Include(x => x.Columns)
          .Select(c => new { c.Id, StrimLinedName = c.StrimLinedName.Replace(" ", ""), c }).ToArray();
      var rels = db.Relations
          .Include(x => x.ForeignColumn.Table)
          .Include(x => x.MasterColumn.Table)
          .ToArray();
      var relMasters = db.Relations.Select(c => new
      {
        ForeginColumn = c.ForeignColumn.StrimLinedName,
        Foreign = c.ForeignColumn.Table.StrimLinedName.Replace(" ", ""),
        Master = Functions.Singularize(c.MasterColumn.Table.StrimLinedName.Replace(" ", "")),
      }).ToArray().GroupBy(x => x.Foreign)
      .ToDictionary(x => x.Key, y => y.Select(r => new { r.Master, r.ForeginColumn }).ToArray());
      var txtIncludes = $@"using Microsoft.EntityFrameworkCore;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using System.Linq;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.EfQueries
{{
    public static class AddIncludes
    {{
        internal static IQueryable<T> GetObjects<T>(IQueryable<T> query) where T : BaseModel, new()
        {{
           // query = query
           //     .Include(x => x.RecordCommentBase)
           //     .Include(x => x.RecordVerificationBase);

{string.Join("\r\n", tables.Select(x =>
{
  var singular = Functions.Singularize(x.StrimLinedName);
  var mx = new StringBuilder($@"            if (typeof(T) == typeof({singular}))
                return (IQueryable<T>)((IQueryable<{singular}>)query)");
  if (relMasters.ContainsKey(x.StrimLinedName))
    foreach (var item in relMasters[x.StrimLinedName])
    {
      var name = item.ForeginColumn.Trim();
      if (name.ToLower().EndsWith(" id"))
        name = name[0..^3].Trim().Replace(" ", "");
      mx.AppendLine().AppendFormat("                    .Include(x => x.{0})", name);
    }
  mx.Append(";");
  return mx;
}))}
            return query;
        }}
    }}
}}
";

      var txtMapped = $@"using AutoMapper;
using AutoMapper.QueryableExtensions;
using Pilgrims.Projects.Assistant.Contracts.Data;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Data.Helpers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using System;
using System.Linq;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.EfQueries
{{
    public static class MappedQueryModel
    {{
        internal static IQueryable<BaseQueryModel> GetObjects(Type type, Contracts.Helpers.PageListParams param = null)
        {{
            try
            {{
                if (type == null) return null;

                if (type.FullName.EndsWith(""QueryModel""))
                {{
                var strName = $""Pilgrims.Projects.Assistant.DataLayer.Data.Models.{{type.Name.Replace(""QueryModel"", """")}}"";
                type = Type.GetType(strName);
            }}

            var db = DataContext.Create();
            IQueryable<Tx> CheckQuery<Tx>(IQueryable<Tx> query) where Tx : BaseModel
            {{
                if (param != null)
                   query = query.CheckFilters(param, true);
                return query;
            }}

{string.Join("\r\n", tables.Select(x => getBody(x.c, rels, true)))}

            throw new BadImageFormatException($""Can't translate the query to type {{type.FullName}}"");
            }}
            catch (Exception ex)
            {{
                throw ex.InnerError();
            }}
       }}
    }}
}}
";


      var txtNative = $@"using Microsoft.EntityFrameworkCore;
using Pilgrims.Projects.Assistant.Data.Helpers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using System;
using System.Linq;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.Classes
{{
    public static class NativeQueryModel
    {{
        internal static IQueryable<BaseModel> GetObjects(Type type, Contracts.Helpers.PageListParams param = null)
        {{
            try
            {{
                if (type == null) return null;

                if (type.Name != ""QueryModel"" && type.FullName.EndsWith(""QueryModel""))
                {{
                    var strName = $""Pilgrims.Projects.Assistant.DataLayer.Data.Models.{{type.Name.Replace(""QueryModel"", """")}}"";
                    type = Type.GetType(strName);
                }}

                var db = DataContext.Create();
                IQueryable<Tx> CheckQuery<Tx>(IQueryable<Tx> query) where Tx : BaseModel
                {{
                    if (param != null)
                        query = query.CheckFilters(param, true);
                    return query;
                }}

                {string.Join("\r\n", tables.Select(x => getBody(x.c, rels, false)))}

            throw new BadImageFormatException($""Can't translate the query to type {{type.FullName}}"");
            }}
            catch (Exception ex)
            {{
                throw ex.InnerError();
            }}
        }}
    }}
}}

";

      var path = Path.Combine(Defaults.MainPath, "EfQueries");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      File.WriteAllText(Path.Combine(path, "AddIncludes.cs"), txtIncludes);
      File.WriteAllText(Path.Combine(path, "MappedQueryModel.cs"), txtMapped);
      File.WriteAllText(Path.Combine(path, "NativeQueryModel.cs"), txtNative);
    }
  }

