using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using PPMS.Console.Models;

namespace PPMS.Console.Generators
{
    public class Repository
    {

        internal static void GenerateContext()
        {
            var sb = new StringBuilder(@"
      
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace PPMS.Console.Data {
  public class Context : DbContextWithTriggers {
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

                var grps = tables.Select(x => x.Group).Distinct().ToArray()
                    .Select(x =>
                    {
                        var spls = x.Replace("/", "\\").Split('\\').Select(m => m.MakeName());
                        return "using PPMS.API.Models." + string.Join(".", spls) + ";";
                    }).ToArray();

                var objs = tables.Select(x => x.Name)
                    .Select(x => string.Format("    public DbSet<{0}> {1} {{ get; set; }}",
                       Functions.Singularize(x).MakeName(), x.MakeName()
                   )).ToArray();

                var mm = string.Join("\r\n", objs);
                sb.Insert(0, string.Join("\r\n", grps));
                sb.Append(mm);
            }
            sb.Append(@"
      
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlite (@""Filename=Database.db"");
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

            path = Path.Combine(path, "DataContext.cs");
            File.WriteAllText(path, sb.ToString());
        }

        internal static void Generate()
        {
            using (var db = new Data.Context())
            {
                var rel = db.Relations.Select(x => x.MasterColumnId).Union(db.Relations.Select(x => x.ForeignColumnId)).Distinct();
                var mCols = db.Columns.Where(x => rel.Contains(x.Id)).Select(x => new { x.ColumnName, x.Id }).Distinct().ToArray();
                var updateCols = mCols.Where(x => !Regex.IsMatch(x.ColumnName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase)).Select(x => new
                {
                    x.Id,
                    Name = x.ColumnName + " ID"
                }).ToArray();

                foreach (var item in updateCols)
                    db.Columns.First(x => x.Id == item.Id).ColumnName = item.Name;

                updateCols = mCols.Where(x => Regex.IsMatch(x.ColumnName, "id +id *$", RegexOptions.IgnoreCase)).Select(x => new
                {
                    x.Id,
                    Name = x.ColumnName.Substring(0, x.ColumnName.Length - 2).Trim()
                }).ToArray();

                foreach (var item in updateCols)
                    db.Columns.First(x => x.Id == item.Id).ColumnName = item.Name;

                db.SaveChanges();
            }
            using (var tsk = Task.Factory.StartNew(GenerateModels))
            {
                using (var tsk2 = Task.Factory.StartNew(GenerateContext))
                {
                    using (var tsk3 = Task.Factory.StartNew(SeedData.GenerateContext))
                    {
                        Task.WaitAll(tsk, tsk2, tsk3);
                        System.Console.WriteLine("Done");
                    }
                }
            }
        }
        internal static void GenerateModels()
        {
            try
            {
                using (var db = new Data.Context())
                {
                    var tables = db.Tables;
                    var allRels = db.Relations.ToArray()
                        .Distinct(new RelationComparer())
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
                        }).Where(x => x.Master != null && x.Foreign != null)
                        .GroupBy(x => string.Format("{0}={1}={2}={3}", x.Master.Id, x.Foreign.Id, x.ForeignColumn, x.MasterColumn))
                        .Select(x => x.First()).ToArray();

                    var dbGroups = db.Groups.ToArray();
                    var dbColumns = db.Columns.ToArray();

                    int tableCount = 1;
                    var sbForeign = new StringBuilder();

                    foreach (var table in tables)
                    {
                        System.Console.WriteLine($"Processing table {table.Name} => {tableCount++} / {tables.Count()}");
                        var groupName = "General";
                        var groupObj = dbGroups.FirstOrDefault(x => x.TableId == table.Id);
                        if (groupObj != null &&
                            !string.IsNullOrWhiteSpace(groupObj.GroupName))
                            groupName = groupObj.GroupName;

                        var importGroups = new List<string>();

                        var group = groupName.Replace("/", "\\").Split('\\')
                            .Select(x => Functions.MakeAllFirstLetterCapital(x, false).Replace(" ", ""))
                            .ToArray();

                        var reffferencesMethods = GetReferencesMethods(table, allRels.Where(x => x.Master.Id == table.Id).Select(x => x.Rel).ToArray());

                        string incl = null;
                        if (allRels.Where(x => x.Foreign.Id == table.Id).Any())
                            incl = string.Join("\r\n", allRels.Where(x => x.Foreign.Id == table.Id)
                                .Select(x =>
                                {
                                    var col = x.ForeignColumn;
                                    if (col.ToLower().EndsWith(" id"))
                                        col = col.Substring(0, col.Length - 3).Trim();

                                    return string.Format(".Include(x => x.{0})", Functions.Singularize(col.MakeName()));
                                }));

                        var name = Functions.MakeAllFirstLetterCapital(table.StrimLinedName, false);
                        var singular = Functions.Singularize(name)?.Replace(" ", "");
                        var tblName = "tbl_" + name?.Replace(" ", "_").ToLower();

                        var sbRepoInterfaces = new StringBuilder();
                        var sbRepos = new StringBuilder();
                        sbRepoInterfaces.AppendFormat(@"using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using PPMS.API.Controllers;
using PPMS.API.Data;
using PPMS.API.Helpers;
using PPMS.API.Models.{1};

namespace PPMS.API.Contracts.Repositories.{1}
{{
    public interface I{0}Repository {{
        Task Add (params {0}[] entities);
        Task<{0}[]> Update (params {0}[] entities);
        Task<{0}[]> Update (params PatchObject[] entities);
        Task UpdateReferences (string oldId, string newId, DataContext db = null);
        public Task<{0}[]> Update (PutObject[] {2}sToUpdate);
        Task<{0}> Update (object entity, string key);
        Task<{0}> Update (JsonPatchDocument entity, string key);
        Task Delete (params {0}[] entities);
        Task Delete (params string[] keys);
        Task<{0}> Get (string {2}Id);
        Task<dynamic> GetReturnObject (string {2}Id, SingleObjectParam param = null);
        Task<PagedList<dynamic>> Get(PageListParams pageListParams = null);
        Task<PagedList<dynamic>> Get(IEnumerable<string> {2}Ids, PageListParams pageListParams = null);",
                                singular, string.Join(".", group), Functions.MakeFirstSmallOtherLetterCapital(singular))
                            .AppendLine();

                        sbRepos.AppendFormat(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using PPMS.API.Contracts.Repositories.ProjectData.ProjectData;
using PPMS.API.Controllers;
using PPMS.API.Data;
using PPMS.API.Helpers;
using PPMS.API.Models;
using PPMS.API.Models.{1};
using PPMS.API.Contracts.Repositories.{1};
using PPMS.API.Queries.{1};
using PPMS.API.Queries.General;
using PPMS.API.Models.General;


namespace PPMS.API.Repositories.{1}
{{
    public class {0}Repository : BaseRepository<{0}>, I{0}Repository
    {{
        private static PropertyInfo[] _propertyInfos;
        public override PropertyInfo[] PropertyInfos
        {{
            get
            {{
                return _propertyInfos ?? (_propertyInfos = typeof({0}).GetPrimitiveProperties());
            }}
        }} 

        public static IQueryable<{0}> AddMasterObject(IQueryable<{0}> query) 
            => query{5};

        public async override Task<{0}> Update(JsonPatchDocument patchDoc, string key)
        {{
            if (string.IsNullOrWhiteSpace(key))
                return null;

            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            var {2} = await AddMasterObject(db.{3}).FirstOrDefaultAsync(x => x.Id == key);
            if ({2} != null)
            {{
                patchDoc.ApplyTo({2});
                if ({2}.Id != key)
                    await UpdateReferences(key, {2}.Id, db);
                db.SaveChanges();
                return {2};
            }}

            return null;
        }}

        public async override  Task<{0}[]> Update(params PatchObject[] entities)
        {{
            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            var tables = new List<{0}>();
            foreach (var doc in entities)
            {{
                if (string.IsNullOrWhiteSpace(doc.Id))
                    continue;

                var {2} = await AddMasterObject(db.{3}).FirstOrDefaultAsync(x => x.Id == doc.Id);
                if ({2} != null)
                {{
                    doc.Data.ApplyTo({2});
                    if ({2}.Id != doc.Id)
                        await UpdateReferences(doc.Id, {2}.Id, db);
                    tables.Add({2});
                }}
            }}

            if (tables.Any())
            {{
                db.SaveChanges();
                return tables.ToArray();
            }}
            return null;
        }}

        {4}

        public async override  Task Add(params {0}[] entity)
        {{
            if (!entity.Any())
                return;

            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            if (entity.Count() == 1)
                db.{3}.Add(entity.First());
            else
                db.{3}.AddRange(entity);
            await db.SaveChangesAsync();
        }}

        public async override  Task Delete(params {0}[] entity)
        {{
            if (!entity.Any())
                return;

            using var db = new DataContext();
                db.PrepareContext(QueryGeneralParam);
                if (entity.Count() == 1)
                    db.{3}.Remove(entity.First());
                else
                    db.{3}.RemoveRange(entity.First());
                await db.SaveChangesAsync();
       }}

        public async override  Task Delete(params string[] keys)
        {{
            if (!keys.Any())
                return;

            keys = keys.Select(x => x.ToUpper()).ToArray();

            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            db.{3}.RemoveRange(db.{3}.Where(x => keys.Contains(x.Id.ToUpper())));
            await db.SaveChangesAsync();
        }}

        public async override  Task<{0}> Get(string {2}Id)
        {{
            if (string.IsNullOrWhiteSpace({2}Id))
                return null;

            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            return await AddMasterObject(db.{3}).FirstOrDefaultAsync(x => x.Id == {2}Id);
        }}

        public async override  Task<PagedList<dynamic>> Get(IEnumerable<string> {2}Ids, PageListParams pageListParams = null)
        {{
            if ({2}Ids == null || !{2}Ids.Any())
                return null;

            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            var query = AddMasterObject(db.{3}).Where(x => {2}Ids.Contains(x.Id));
            query = query.CheckFilters(PropertyInfos, pageListParams);
            return await query.GetReturnObjects<{0}, {0}Query>(PropertyInfos, pageListParams);
        }}

        public async override  Task<dynamic> GetReturnObject(string {2}Id, SingleObjectParam param = null)
        {{
            if (string.IsNullOrWhiteSpace({2}Id))
                return null;

            using var db = new DataContext();
                db.PrepareContext(this.QueryGeneralParam);
                return await AddMasterObject(db.{3}).Where(x => x.Id == {2}Id)
                    .GetReturnObject<{0}, {0}Query>(PropertyInfos, param);
         }}

        public async override  Task<PagedList<dynamic>> Get(PageListParams pageListParams = null)
        {{
            using var db = new DataContext();
                db.PrepareContext(this.QueryGeneralParam);
                IQueryable<{0}> query = AddMasterObject(db.{3});
                query = query.CheckFilters(PropertyInfos, pageListParams);
                return await query.GetReturnObjects<{0}, {0}Query>(PropertyInfos, pageListParams);
        }}
        public async override  Task<{0}[]> Update(params {0}[] entities)
        {{
            return await entities.UpdateModels(PropertyInfos, this.QueryGeneralParam);
        }}

        public async override  Task<{0}> Update(object entity, string key)
        {{
            if (string.IsNullOrWhiteSpace(key))
                return null;

            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            var {2} = await AddMasterObject(db.{3}).FirstOrDefaultAsync(x => x.Id == key);
            {2}.MapForUpdate(entity, PropertyInfos);
            if ({2}.Id != key)
                await UpdateReferences(key, {2}.Id, db);
            db.SaveChanges();
            return {2};
        }}

        public async override  Task<{0}[]> Update(PutObject[] {2}sToUpdate)
        {{
            var {2}s = new List<{0}>();
            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            foreach (var entity in {2}sToUpdate)
            {{
                if (string.IsNullOrWhiteSpace(entity.Id))
                    continue;

                var {2} = await AddMasterObject(db.{3}).FirstOrDefaultAsync(x => x.Id == entity.Id);
                if ({2} == null)
                    continue;

                {2}.MapForUpdate(entity.Obj, PropertyInfos);
                if ({2}.Id != entity.Id)
                    await UpdateReferences(entity.Id, {2}.Id, db);
                {2}s.Add({2});
            }}
            db.SaveChanges();
            return {2}s.ToArray();
        }}

         ",
                                singular, string.Join(".", group),
                                Functions.MakeFirstSmallOtherLetterCapital(singular), name.MakeName(), reffferencesMethods, incl)
                            .AppendLine();

                        var columns = dbColumns.Where(x => x.TableId == table.Id).ToArray();
                        foreach (var column in columns)
                        {

                            var rels = allRels.Where(x => x.Rel.MasterColumnId == column.Id || x.Rel.ForeignColumnId == column.Id).ToArray();
                            var colName = Functions.MakeAllFirstLetterCapital(column.StrimLinedName, false)
                                .Replace(" ", "");

                            if (rels.Any())
                            {
                                //sb.AppendFormat ("    public string {0} {{ get; set; }}", colName).AppendLine ();

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

                                    if (rel.Master.Id == rel.Foreign.Id)
                                    {
                                        var type = Functions.Singularize(rel.Master.Name);
                                        sbRepoInterfaces.AppendFormat("         Task<dynamic> Get{1}(string {2}Id, SingleObjectParam param = null);",
                                                type.MakeName(), propName, Functions.MakeFirstSmallOtherLetterCapital(singular.MakeName()))

                                            .AppendLine();

                                        sbRepos.AppendFormat(@"
             public async Task<dynamic> Get{1}(string {2}Id, SingleObjectParam param = null)
            {{
                using var db = new DataContext();
                    db.PrepareContext(QueryGeneralParam);
                    return await (from obj in db.{3}
                                  from {5} in db.{4}
                                  where {5}.Id == obj.{6}
                                  && {5}.Id == {2}Id
                                  select {5})
                                 .GetReturnObject<{0}, {0}Query> (PropertyInfos, param);
            }}
            
            ", type.MakeName(), propName,
                                            Functions.MakeFirstSmallOtherLetterCapital(singular.Replace(" ", "").MakeName()),
                                            name.MakeName(),
                                            rel.Master.Name.MakeName(),
                                            Functions.MakeFirstSmallOtherLetterCapital(type.Replace(" ", "")).Replace("group", "grp"),
                                            rel.ForeignColumn.MakeName()
                                        );
                                    }
                                    else if (rel.Master.Id == table.Id)
                                    {
                                        // var type = Functions.Singularize (rel.Foreign.Name);
                                        // sb.AppendFormat (@"    [ForeignKey(nameof({0}))]", colName).AppendLine ();
                                        // sb.AppendFormat ("    public ICollection<{0}> {1} {{ get; set; }}", type.MakeName (), rel.Master.Name.MakeName ()).AppendLine ();
                                    }
                                    else
                                    {
                                        importGroups.Add(dbGroups.FirstOrDefault(x => x.TableId == rel.Master.Id)?.GroupName);
                                        var type = Functions.Singularize(rel.Master.Name);
                                        sbRepoInterfaces.AppendFormat("         Task<dynamic> Get{1}(string {2}Id, SingleObjectParam param = null);",
                                                type.MakeName(), propName, Functions.MakeFirstSmallOtherLetterCapital(singular.MakeName()))
                                            .AppendLine();

                                        sbRepos.AppendFormat(@"
            public async Task<dynamic> Get{1}(string {2}Id, SingleObjectParam param = null)
            {{
                using var db = new DataContext();
                    db.PrepareContext(QueryGeneralParam);
                    return await (from {2} in db.{3}
                                  from {5} in db.{4}
                                  where {5}.Id == {2}.{6}
                                  && {5}.Id == {2}Id
                                  select {5})
                                 .GetReturnObject<{0}, {0}Query> (PropertyInfos, param);
            }}
            
            ", type?.MakeName(), propName,
                                            Functions.MakeFirstSmallOtherLetterCapital(singular?.MakeName())?.Replace("group", "grp"),
                                            name?.MakeName(),
                                            rel.Master?.Name?.MakeName(),
                                            Functions.MakeFirstSmallOtherLetterCapital(type?.Replace(" ", ""))?.Replace("group", "grp"),
                                            rel.ForeignColumn?.MakeName()
                                        );
                                    }
                                }
                            }
                        }

                        var allRelations = allRels.Where(x => x.Master.Id == table.Id).ToArray();
                        var donePropNames = new List<string>();
                        var relCols = allRelations.GroupBy(x => x.Foreign.Id)
                            .SelectMany(rels =>
                            {
                                var str = new List<string>();
                                foreach (var rel in rels)
                                {
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

                                    if (!propName.EndsWith("s"))
                                    {
                                        propName = propName + "s";
                                    }

                                    // if(donePropNames.Contains(propName))
                                    // continue;

                                    donePropNames.Add(propName);

                                    var xNameUpper = Functions.MakeAllFirstLetterCapital(rel.Foreign.Name.MakeName(), false);
                                    var xNameLower = Functions.MakeFirstSmallOtherLetterCapital(xNameUpper);

                                    var importGroup = dbGroups.FirstOrDefault(x => x.TableId == rel.Foreign.Id)?.GroupName;
                                    importGroups.Add(importGroup);
                                    var type = Functions.Singularize(rel.Foreign.Name);
                                    str.Add(string.Format(@"     Task<PagedList<dynamic>> Get{1}(string {2}Id, PageListParams pageListParams = null);",
                                        type.MakeName(), propName, Functions.MakeFirstSmallOtherLetterCapital(type.MakeName())));

                                    sbRepos.AppendFormat(@"
            public async Task<PagedList<dynamic>> Get{1}(string {2}Id, PageListParams pageListParams = null)
            {{
                using var db = new DataContext();
                    db.PrepareContext(QueryGeneralParam);
                    var query = db.{3}.Where(x => x.{4} == {2}Id);
                    query = query.CheckFilters(PropertyInfos, pageListParams);                    
                    return await query.GetReturnObjects<{0},{0}Query>(PropertyInfos, pageListParams); 
            }}
", type.MakeName(), propName,
                                        Functions.MakeFirstSmallOtherLetterCapital(type.MakeName()), rel.Foreign.Name.MakeName(), rel.ForeignColumn.MakeName());
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
                                for (int i = 0; i <group.Length; i++)
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

                            if (mmImports.Any())
                            {
                                //sbRepoInterfaces.Insert (0, string.Join (";\r\nusing PPMS.API.Models.", mmImports) + ";\r\n").Insert (0, "using PPMS.API.Models.");

                                var str = "using PPMS.API.Models." + string.Join(";\r\nusing PPMS.API.Models.", mmImports) + ";\r\n";
                                str += str.Replace("PPMS.API.Models", "PPMS.API.Queries");
                                // mmImports = mmImports.Union(mmImports.Select(x => x.Replace("PPMS.API.Models","PPMS.API.Queries"))).ToArray();
                                sbRepos.Insert(0, str);
                            }
                        }

                        foreach (var rel in relCols)
                        {
                            sbRepoInterfaces.AppendLine(rel);
                            // sbRepos.AppendLine (rel);
                        }
                        sbRepoInterfaces.AppendLine("    }");
                        sbRepoInterfaces.AppendLine("}");

                        sbRepos.AppendLine("    }");
                        sbRepos.AppendLine("}");

                        var path = Path.Combine(Defaults.MainPath, "RepositoryContracts", string.Join("/", group));
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        path = Path.Combine(path, string.Format("I{0}Repository.cs", singular));
                        sbRepoInterfaces = sbRepoInterfaces.Replace("public Autoincrement ", "public General.Autoincrement ");
                        sbRepoInterfaces = sbRepoInterfaces.Replace("<ProjectOption>", "<Models.ProjectOption.ProjectOptions.ProjectOption>");
                        sbRepoInterfaces = sbRepoInterfaces.Replace("using PPMS.API.Models.;", "");
                        sbRepoInterfaces = sbRepoInterfaces.Replace("public DataType ", "public Settings.Project.DataSettings.DataType ");
                        sbRepoInterfaces = sbRepoInterfaces.Replace("public Project ", "public System.General.Project ");
                        File.WriteAllText(path, sbRepoInterfaces.ToString());

                        path = Path.Combine(Defaults.MainPath, "Repositories", string.Join("/", group));
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        path = Path.Combine(path, string.Format("{0}Repository.cs", singular));
                        sbRepos = sbRepos.Replace("public Autoincrement ", "public General.Autoincrement ");
                        sbRepos = sbRepos.Replace("<ProjectOption>", "<Models.ProjectOption.ProjectOptions.ProjectOption>");
                        sbRepos = sbRepos.Replace("using PPMS.API.Models.;", "");
                        sbRepos = sbRepos.Replace("public DataType ", "public Settings.Project.DataSettings.DataType ");
                        sbRepos = sbRepos.Replace("public Project ", "public System.General.Project ");
                        File.WriteAllText(path, sbRepos.ToString());

                        if (allRelations.Any())
                            sbForeign.AppendFormat(@"
                }}
            }};");
                        // if (tableCount > 5)
                        //     break;
                    }
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("Error: " + exception.ToString());
            }
        }

        private static string GetReferencesMethods(DatabaseTable table, TableRelation[] allRels)
        {
            if (allRels == null || !allRels.Any())
            {
                return @" ";
            }

            var objs = allRels.Select(x =>
            {
                var updateText =
                    string.Format(@"                db.{0}.Where(x => oldId == x.{1}).ToList().ForEach(x => x.{1} = newId);",
                        x.ForeignColumn.Table.Name.MakeName(), x.ForeignColumn.ColumnName.MakeName());

                var hasRefsText = string.Format(@"(db.{0}.Where(x => ids.Contains(x.{1})).Select(x => x.Id))",
                    x.ForeignColumn.Table.Name.MakeName(), x.ForeignColumn.ColumnName.MakeName());

                var refsTexts = string.Format(@"(db.{0}.Where(x => ids.Contains(x.{1}))
                    .Select(x => ""{2}  "" + GeneralSeparator + "" "" + x.Id))",
                    x.ForeignColumn.Table.Name.MakeName(), x.ForeignColumn.ColumnName.MakeName(), x.ForeignColumn.Table.OriginalName);

                return new Tuple<string, string, string>(updateText, hasRefsText, refsTexts);
            });

            return string.Format(@"public async override  Task UpdateReferences(string oldId, string newId, DataContext db = null)
        {{
            var createNewDb = db == null;
            try
            {{
                if (createNewDb)
                    db = new DataContext();

{0}

                if (createNewDb)
                    await db.SaveChangesAsync();
            }}
            finally
            {{
                if (createNewDb)
                    db?.Dispose();
            }}
        }}

        internal protected async override Task<bool> HasRefferences(params string[] ids)
        {{
            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            return await {1}
                .AnyAsync();
        }}

        internal protected async override Task<Dictionary<string, string[]>> GetRefferences(params string[] ids)
        {{
            using var db = new DataContext();
            db.PrepareContext(QueryGeneralParam);
            var objs = await {2}
                .ToArrayAsync();

            return objs.Select(x => global::System.Text.RegularExpressions.Regex.Split(x, GeneralSeparator))
                .GroupBy(x => x[0])
                .ToDictionary(x => x.Key, y => y.Select(m => m[1]).ToArray());
        }}", string.Join("\r\n", objs.Select(x => x.Item1)),
                string.Join("\r\n                .Union", objs.Select(x => x.Item2)),
                string.Join("\r\n                .Union", objs.Select(x => x.Item3)));

        }
    }
}