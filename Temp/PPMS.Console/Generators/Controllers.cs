using System;
using System.Collections.Generic;
using System.Data.Common;
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
    public class Controllers
    {

        internal static void Generate()
        {
            GenerateServiceRegister();
            GenerateControllers(); // GenerateDtos();
            // using (var tsk = Task.Factory.StartNew (GenerateDtos)) {
            //     using (var tsk2 = Task.Factory.StartNew (GenerateQueryMakers)) {
            //         using (var tsk3 = Task.Factory.StartNew (GenerateControllers)) {
            //             Task.WaitAll (tsk, tsk2, tsk3);
            //             System.Console.WriteLine ("Done");
            //         }
            //     }
            // }
        }

        internal static void GenerateServiceRegister()
        {
            string[] imports = { };
            string[] singularTables = { };

            using (var db = new Data.Context())
            {
                imports = (
                        from table in db.Tables from grp in db.Groups where grp.TableId == table.Id select grp.GroupName
                    ).Distinct().ToArray()
                    .Select(groupName => groupName.Replace("/", "\\").Split('\\')
                       .Select(x => Functions.MakeAllFirstLetterCapital(x, false).Replace(" ", ""))
                       .ToArray()).Select(x =>
                       {
                           return string.Format(@"
using PPMS.API.Contracts.Repositories.{0};
using PPMS.API.Repositories.{0};", string.Join(".", x));
                       }).ToArray();
                singularTables = db.Tables.ToArray().Select(x => Functions.Singularize(x.Name))
                    .Select(x => string.Format(@"
            services.AddScoped<I{0}Repository, {0}Repository>();", x.MakeName())).ToArray();
            }
            var reg = @"using System;
using Microsoft.Extensions.DependencyInjection;
using PPMS.API.Contracts.Repositories.General;
using PPMS.API.Repositories.General;
{0}

namespace PPMS.API.ServiceRegisters
{{
    public static class RepositoriesRegister
    {{
        internal static void RegisterRepos(IServiceCollection services)
        {{
{1}   
        }}
    }}
}}";
            var tbls = string.Format(reg, string.Join("", imports), string.Join("", singularTables));
            var path = Path.Combine(Defaults.MainPath, "Commons");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var txt = Functions.CheckCSharpFileContent(tbls);
            path = Path.Combine(path, "RepositoriesRegister.cs");
            File.WriteAllText(path, txt);
        }

        internal static void GenerateDtos()
        {
            using (var db = new Data.Context())
            {
                var dbGroups = db.Groups.ToArray();
                var dbColumns = db.Columns.ToArray();
                var relColIds = new string[0];
                var allRels = db.Relations.ToArray()
                    .Distinct(new RelationComparer())
                    .Where(x => x != null)
                    .Select(x =>
                    {
                        DatabaseTable master = null;
                        string[] @group = null;
                        try
                        {
                            master = db.Tables.FirstOrDefault(y => y.Id == db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId).TableId);
                        }
                        catch { }
                        try
                        {
                            var groupName = "General";
                            var groupObj = dbGroups.FirstOrDefault(x => x.TableId == master?.Id);
                            if (groupObj != null &&
                                !string.IsNullOrWhiteSpace(groupObj.GroupName))
                                groupName = groupObj.GroupName;

                            @group = groupName.Replace("/", "\\").Split('\\')
                                .Select(x => Functions.MakeAllFirstLetterCapital(x, false).Replace(" ", ""))
                                .ToArray();
                        }
                        catch { }

                        return new
                        {
                            Rel = x,
                            Group = @group,
                            Master = master,
                            MasterColumn = dbColumns.FirstOrDefault(m => m.Id == x.MasterColumnId)?.ColumnName,
                            ForeignColumn = dbColumns.FirstOrDefault(m => m.Id == x.ForeignColumnId)?.ColumnName,
                            ForeignTableId = dbColumns.FirstOrDefault(m => m.Id == x.ForeignColumnId)?.TableId
                        };
                    }).Where(x => x.Master != null).ToArray();

                var sbDtoAutoMapper = new StringBuilder();
                relColIds = allRels.SelectMany(x => new[] { x.Rel.MasterColumnId, x.Rel.ForeignColumnId }).ToArray();

                foreach (var table in db.Tables)
                {
                    var groupName = "General";
                    var groupObj = dbGroups.FirstOrDefault(x => x.TableId == table.Id);
                    if (groupObj != null &&
                        !string.IsNullOrWhiteSpace(groupObj.GroupName))
                        groupName = groupObj.GroupName;

                    var rels = allRels.Where(x => x.ForeignTableId == table.Id).ToArray();
                    var currentColumns = dbColumns.Where(m => m.TableId == table.Id).ToArray();

                    var importGroups = new List<string>();

                    var @group = groupName.Replace("/", "\\").Split('\\')
                        .Select(x => Functions.MakeAllFirstLetterCapital(x, false).Replace(" ", ""))
                        .ToArray();

                    var name = Functions.MakeAllFirstLetterCapital(table?.StrimLinedName, false);
                    var singular = Functions.Singularize(name)?.Replace(" ", "");

                    var cols = currentColumns
                        .Select(column =>
                        {
                            var colName = (column.IsPrimary ? "Id" : column?.ColumnName?.MakeName()) ?? "";
                            var col = string.Format("                    qCol.{0},", colName);
                            return col;
                        }).Distinct().ToList();

                    foreach (var rel in rels)
                    {
                        var colName = rel.ForeignColumn;
                        if (colName.ToLower().Trim().EndsWith(" id"))
                            colName = colName.Trim().Substring(0, colName.Length - 3);

                        colName = string.Format("{0}_Caption", colName.MakeName());

                        var relName = GetRelName(dbColumns.Where(x => x.TableId == rel.Master.Id).ToArray(), rel.Master, rel.ForeignColumn);
                        var value = string.Format("                    {0} = qCol.{1},", colName, relName);
                        if (!cols.Contains(value))
                            cols.Add(value);
                    }

                    var nm = string.Join(";\r\nusing PPMS.API.Models.", rels.Select(x => string.Join(".", x.Group)).Distinct());

                    if (!nm.Contains(string.Join(".", @group)))
                    {
                        nm += $"\r\nusing PPMS.API.Models.{string.Join(".", @group)};";
                    }
                    var sb = new StringBuilder();
                    sb.AppendFormat(@"
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PPMS.API.Data;
using PPMS.API.Helpers;
using PPMS.API.Queries;
{3}

namespace PPMS.API.Queries.{0} {{
    public class {1}Query : IControllerQueryBase<{1}> {{
        public async Task<Helpers.PagedList<dynamic>> CreateReturnList (IQueryable<{1}> query, PageListParams pageListParams = null) {{
            var ctx = query.Select (qCol => new {{
{2}
            }});
            return await PagedList<dynamic>.CreateAsync (ctx, pageListParams?.PageNumber, pageListParams?.PageSize);
        }}

        public async Task<dynamic> CreateReturnObject (IQueryable<{1}> query) {{
            return await query.Select (qCol => new {{
{2}
            }}).FirstOrDefaultAsync ();
        }}
    }}
}}
", string.Join(".", @group), singular, string.Join("\r\n", cols),
                        string.IsNullOrWhiteSpace(nm) ? nm : "\r\nusing PPMS.API.Models." + nm);

                    var path = Path.Combine(Defaults.MainPath, "Queries", string.Join("/", @group));
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    System.Console.WriteLine(string.Format("Writting {0}Query.cs", singular));

                    var txt = Functions.CheckCSharpFileContent(sb.ToString());
                    path = Path.Combine(path, string.Format("{0}Query.cs", singular));
                    File.WriteAllText(path, txt);

                    var sbDtoCols = new StringBuilder();

                    var dtoCols = currentColumns
                        .Select(column =>
                        {

                            // if (column.ColumnName.ToLower ().Contains (" id") || column.ColumnName.ToLower () == "id")

                            if (relColIds.Contains(column.Id))
                            {
                                var colName = column.IsPrimary ? "Id" : column.ColumnName.MakeName();
                                var col = $"    public string {colName} {{ get; set; }}";
                                return col;
                            }
                            else
                            {
                                if (column.Type.ToString().ToLower() == "string")
                                {
                                    var colName = column.IsPrimary ? "Id" : column.ColumnName.MakeName();
                                    var col = $"    public {column.Type} {colName} {{ get; set; }}";
                                    return col;
                                }
                                else
                                {
                                    var colName = column.IsPrimary ? "Id" : column.ColumnName.MakeName();
                                    var col = $"    public {column.Type}? {colName} {{ get; set; }}";
                                    return col;
                                }
                            }
                        });

                    var sbFrom = new StringBuilder();
                    var sbTo = new StringBuilder();
                    sbFrom.AppendFormat(@"namespace PPMS.API.Dtos.{0}
{{
    public class {1}FromClientDto: BaseDto
    {{
{2}
    }}
}}
", string.Join(".", group), singular, string.Join("\r\n", dtoCols));

                    if (sbDtoAutoMapper.Length > 20)
                        sbDtoAutoMapper.AppendFormat(@";
");

                    sbDtoAutoMapper.AppendFormat(@"
            CreateMap <{0}FromClientDto, {0}>();
            CreateMap<{0}, {0}ToClientDto>()", singular);

                    var processedDtoRels = new List<string>();
                    foreach (var rel in rels)
                    {
                        var mm = $"{rel.MasterColumn}<==>{rel.ForeignColumn}";
                        if (processedDtoRels.Contains(mm))
                            continue;

                        processedDtoRels.Add(mm);

                        var colName = rel.ForeignColumn;
                        if (colName.ToLower().Trim().EndsWith(" id"))
                            colName = colName.Trim().Substring(0, colName.Length - 3);

                        colName = string.Format("{0}_Caption", colName.MakeName());
                        sbDtoCols.AppendFormat("    public string {0} {{ get; set; }}\r\n", colName.Replace(" ", "_"));

                        var relName = GetRelName(dbColumns.Where(x => x.TableId == rel?.Master?.Id).ToArray(), rel?.Master, rel?.ForeignColumn);
                        sbDtoAutoMapper.AppendFormat(@"
                  .ForMember(dest => dest.{0}, opt => opt.MapFrom(src => src.{1}))", colName?.Replace(" ", "_"), relName?.Replace(".", "?."));
                        // sbDtoCols.AppendFormat ("    string {0}", relName).AppendLine ();
                        // var value = string.Format ("                    {0} = qCol.{1},", relName.Replace (".", "_"), relName);
                        // if (!cols.Contains (value))
                        //     cols.Add (value);
                        try
                        {
                            WriteDtoAutoMapperProfile(sbDtoAutoMapper.ToString());
                        }
                        catch { }
                    }
                    sbDtoAutoMapper.AppendLine(".IgnoreAllPropertiesWithAnInaccessibleSetter ();");

                    sbTo.AppendFormat(@"namespace PPMS.API.Dtos.{0}
{{
    public class {1}ToClientDto: BaseDto
    {{
{2}
{3}    }}
}}
", string.Join(".", group), singular, string.Join("\r\n", dtoCols), sbDtoCols);

                    path = Path.Combine(Defaults.MainPath, "Dtos", string.Join("/", group));
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    System.Console.WriteLine(string.Format("Writting {0}FromClientDto.cs", singular));

                    File.WriteAllText(Path.Combine(path, string.Format("{0}FromClientDto.cs", singular)), sbFrom.ToString());
                    File.WriteAllText(Path.Combine(path, string.Format("{0}ToClientDto.cs", singular)), sbTo.ToString());
                }

                WriteDtoAutoMapperProfile(sbDtoAutoMapper.ToString());
            }
        }

        private static void WriteDtoAutoMapperProfile(string content)
        {
            var body = @"using AutoMapper;
using PPMS.API.Dtos.DesignAndPlanning.Queries;
using PPMS.API.Dtos.DesignAndPlanning.Reports;
using PPMS.API.Dtos.General;
using PPMS.API.Dtos.Implimentations.Diagrams;
using PPMS.API.Dtos.Implimentations.Solution;
using PPMS.API.Dtos.Implimentations.Structures;
using PPMS.API.Dtos.ProjectData.DevelopmentGuidelines;
using PPMS.API.Dtos.ProjectData.Forms.Controls;
using PPMS.API.Dtos.ProjectData.Forms.Forms;
using PPMS.API.Dtos.ProjectData.GeneralData;
using PPMS.API.Dtos.ProjectData.GeneralData.Encryptions;
using PPMS.API.Dtos.ProjectData.GeneralData.UniqueCombinations;
using PPMS.API.Dtos.ProjectData.ProjectData;
using PPMS.API.Dtos.ProjectOption.DevelopmentGuidelines;
using PPMS.API.Dtos.ProjectOption.Estimations;
using PPMS.API.Dtos.ProjectOption.ProjectOptions;
using PPMS.API.Dtos.ProjectOption.Structures;
using PPMS.API.Dtos.Requirements.DevelopmentTests;
using PPMS.API.Dtos.Requirements.General;
using PPMS.API.Dtos.Requirements.UseCasesAndUML;
using PPMS.API.Dtos.Requirements.UserStories;
using PPMS.API.Dtos.Settings.EntrySettings;
using PPMS.API.Dtos.Settings.General;
using PPMS.API.Dtos.Settings.Project.DataSettings;
using PPMS.API.Dtos.System.General;
using PPMS.API.Dtos.System.UserManagement;
using PPMS.API.Dtos.System.UserManagement.Audit;
using PPMS.API.Models.DesignAndPlanning.Queries;
using PPMS.API.Models.DesignAndPlanning.Reports;
using PPMS.API.Models.General;
using PPMS.API.Models.Implimentations.Diagrams;
using PPMS.API.Models.Implimentations.Solution;
using PPMS.API.Models.Implimentations.Structures;
using PPMS.API.Models.ProjectData.DevelopmentGuidelines;
using PPMS.API.Models.ProjectData.Forms.Controls;
using PPMS.API.Models.ProjectData.Forms.Forms;
using PPMS.API.Models.ProjectData.GeneralData;
using PPMS.API.Models.ProjectData.GeneralData.Encryptions;
using PPMS.API.Models.ProjectData.GeneralData.UniqueCombinations;
using PPMS.API.Models.ProjectData.ProjectData;
using PPMS.API.Models.ProjectOption.DevelopmentGuidelines;
using PPMS.API.Models.ProjectOption.Estimations;
using PPMS.API.Models.ProjectOption.ProjectOptions;
using PPMS.API.Models.ProjectOption.Structures;
using PPMS.API.Models.Requirements.DevelopmentTests;
using PPMS.API.Models.Requirements.General;
using PPMS.API.Models.Requirements.UseCasesAndUML;
using PPMS.API.Models.Requirements.UserStories;
using PPMS.API.Models.Settings.EntrySettings;
using PPMS.API.Models.Settings.General;
using PPMS.API.Models.Settings.Project.DataSettings;
using PPMS.API.Models.System.General;
using PPMS.API.Models.System.UserManagement;
using PPMS.API.Models.System.UserManagement.Audit;
using PPMS.API.Models.SystemModels;

namespace PPMS.API.Dtos {{
    public class DtoProfiles : Profile {{

        public DtoProfiles () {{
 {0}
       }}
       }}
       }}
";

            content = string.Format(body, content);
            File.WriteAllText(Path.Combine(Defaults.MainPath, "Dtos", "DtoProfiles.cs"), content);
        }

        private static string GetRelName(TableColumn[] tableColumns, DatabaseTable master, string colName)
        {
            return Functions.GetRelName(tableColumns, master, colName);
        }

        internal static void GenerateQueryMakers()
        {
            using (var db = new Data.Context())
            {
                var dbGroups = db.Groups.ToArray();
                var dbColumns = db.Columns.ToArray();

                foreach (var table in db.Tables)
                {
                    var groupName = "General";
                    var groupObj = dbGroups.FirstOrDefault(x => x.TableId == table.Id);
                    if (groupObj != null &&
                        !string.IsNullOrWhiteSpace(groupObj.GroupName))
                        groupName = groupObj.GroupName;

                    var importGroups = new List<string>();

                    var group = groupName.Replace("/", "\\").Split('\\')
                        .Select(x => Functions.MakeAllFirstLetterCapital(x, false).Replace(" ", ""))
                        .ToArray();

                    var name = Functions.MakeAllFirstLetterCapital(table.StrimLinedName, false);
                    var singular = Functions.Singularize(name).Replace(" ", "");
                    var tblName = "tbl_" + name.Replace(" ", "_").ToLower();

                }
            }
        }
        internal static void GenerateControllers()
        {
            using (var db = new Data.Context())
            {
                var dbGroups = db.Groups.ToArray();
                var dbColumns = db.Columns.ToArray();

                foreach (var table in db.Tables)
                {
                    var groupName = "General";
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

                    var cols = dbColumns.Where(x => x.TableId == table.Id)
                        .Select(column =>
                        {
                            var colName = column.IsPrimary ? "Id" : column.ColumnName.MakeName();
                            var col = string.Format("    public {1} {0} {{ get; internal set; }}", colName, column.Type);
                            return col;
                        });

                    var sb = new StringBuilder(string.Format(controllerBody, string.Join(".", group), singular,
                        name.MakeName(), Functions.MakeFirstSmallOtherLetterCapital(name.MakeName()), name, name.ToLower(),
                        name.ToLower().Replace(" ", "-")));

                    var allRelations = db.Relations.ToArray()
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
                        .Where(x => x.Master.Id == table.Id || x.Foreign.Id == table.Id)
                        .GroupBy(x => string.Format("{0}={1}={2}={3}", x.Master.Id, x.Foreign.Id, x.ForeignColumn, x.MasterColumn))
                        .Select(x => x.First()).ToArray();

                    var relCols = allRelations.GroupBy(x => x.Foreign.Id)
                        .SelectMany(rels =>
                        {
                            var str = new List<string>();
                            foreach (var rel in rels)
                            {

                                var subRoute = rel.ForeignColumn;
                                if (Regex.IsMatch(subRoute, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
                                {
                                    var len = Regex.Match(subRoute,
                                              "(id|code|no|number|key) *$",
                                              RegexOptions.IgnoreCase).Value.Length;
                                    subRoute = subRoute.Trim().Substring(0, subRoute.Length - len);
                                    subRoute = Functions.Singularize(subRoute)?.ToLower().Trim().Replace(" ", "-");
                                }

                                var colName = Functions.MakeAllFirstLetterCapital(rel.ForeignColumn, false)
                                    .Replace(" ", "");

                                var propName = colName;
                                if (Regex.IsMatch(propName, "(id|code|no|number|key) *$", RegexOptions.IgnoreCase))
                                {
                                    var len = Regex.Match(propName,
                                           "(id|code|no|number|key) *$",
                                           RegexOptions.IgnoreCase).Value.Length;
                                    propName = propName.Trim().Substring(0, propName.Length - len);
                                    propName = propName.MakeName();
                                }

                                if (rel.Master.Id != table.Id || rel.Master.Id == rel.Foreign.Id)
                                {
                                    var type = Functions.Singularize(rel.Master.Name.Replace(" ", "-"));
                                    str.Add(string.Format(@" 
       [HttpGet (""{{id}}/{1}"", Name = ""Get{4}_{0}"")]
        public async Task<IActionResult> Get{0} (string id,  [FromQuery]SingleObjectParam param = null) {{
           if (!CheckAuthorization (""{2} - GetOne - {3}/"" + id, id, out string message))
             return Unauthorized (message);   

            return Ok (await _repo.Get{0}(id, param));
        }}


", propName, subRoute, name, rel.Master.Name, singular.MakeName()));
                                }
                            }
                            return str;
                        }).Distinct().ToList();

                    var relColsForeigns = allRelations.GroupBy(x => x.Foreign.Id)
                        .SelectMany(rels =>
                        {
                            var str = new List<string>();
                            foreach (var rel in rels)
                            {
                                //var propName = rel.ForeignColumn;

                                if (rel.Master.Id == table.Id && rel.Master.Id != rel.Foreign.Id)
                                {
                                    var propName = rel.Foreign.Name;
                                    var lss = rels.ToArray();
                                    //System.Console.WriteLine ("Count " + rel.Master.Name + " " + lss.Count ());
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
                                    var subRoute = propName.ToLower().Trim().Replace(" ", "-");
                                    subRoute = Functions.Pluralize(subRoute);

                                    propName = propName.MakeName();

                                    if (!propName.EndsWith("s"))
                                    {
                                        propName = propName + "s";
                                    }

                                    var xNameUpper = Functions.MakeAllFirstLetterCapital(rel.Foreign.Name.MakeName(), false);
                                    var xNameLower = Functions.MakeFirstSmallOtherLetterCapital(xNameUpper);

                                    var importGroup = dbGroups.FirstOrDefault(x => x.TableId == rel.Foreign.Id)?.GroupName;
                                    importGroups.Add(importGroup);
                                    var type = Functions.Singularize(rel.Foreign.Name);
                                    str.Add(string.Format(@"       
        [HttpGet (""{{id}}/{2}"", Name = ""Get{6}_{0}"")]
        public async Task<IActionResult> Get{0} ([FromQuery] PageListParams pageListParams, string id) {{
            if (!CheckAuthorization (""{3} - Get - {0}/"" + id, pageListParams, out string message))
             return Unauthorized (message);   

            var {1} = await _repo.Get{0}(id, pageListParams);
       
            Response.AddPagination({1}.CurrentPage, {1}.PageSize,
                {1}.TotalCount, {1}.TotalPages);
            return Ok({1});
        }}
        
        ", propName, rel.Foreign.Name.Replace(" ", "").MakeName(),
                                        subRoute,
                                        singular, type.MakeName(), propName, singular.MakeName()));
                                }
                            }
                            return str;
                        }).Distinct().ToArray();

                    relCols.AddRange(relColsForeigns);
                    foreach (var rel in relCols.Distinct())
                    {
                        sb.AppendLine(rel);
                    }
                    sb.AppendLine("}").AppendLine("}");

                    var path = Path.Combine(Defaults.MainPath, "Controllers", string.Join("/", group));
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    System.Console.WriteLine(string.Format("Writting {0}Controller.cs", singular));

                    path = Path.Combine(path, string.Format("{0}Controller.cs", name.MakeName()));
                    File.WriteAllText(path, sb.ToString());
                    sb.Clear();
                }
            }
        }
        const string controllerBody = @"using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PPMS.API.Contracts.Repositories.{0};
using PPMS.API.Dtos.{0};
using PPMS.API.Helpers;
using PPMS.API.Models.{0};
using PPMS.API.Repositories;

namespace PPMS.API.Controllers.{0}
{{
    [Route(""api/{6}"")]
    public class {2}Controller : BaseController<{1}>
    {{
        I{1}Repository _repo;
        internal override string ControllerName {{ get {{ return ""{4}""; }} }}
        public {2}Controller(IMapper mapper, I{1}Repository repo)
        {{
            this.Mapper = mapper;
            _repo = repo;
            if (_repo is BaseRepository<{1}>)
                Repository = (BaseRepository<{1}>)repo;
        }}

        [HttpGet]
        public async Task<IActionResult> Get{2}([FromQuery] PageListParams pageListParams)
        {{
            if (!CheckAuthorization(""{4} - Get"", pageListParams, out string message))
                return Unauthorized(message);

            var {3} = await _repo.Get(pageListParams);
            Response.AddPagination({3}.CurrentPage, {3}.PageSize,
                {3}.TotalCount, {3}.TotalPages);

            return Ok({3});
        }}

       [HttpGet (""by-ids"")]
        public async Task<IActionResult> Get{2}([FromBody] object model, [FromQuery] PageListParams pageListParams)
        {{
            if (!CheckAuthorization(""{4} - Get"", pageListParams, out string message))
                return Unauthorized(message);

            var json = model?.ToString();
            if (string.IsNullOrWhiteSpace(json))
            {{
                throw new InvalidDataException(""Please data you send is not valid; i.e empty"");
            }}

            var ids = json.IsJsonArrray() ?
                JsonConvert.DeserializeObject<string[]>(json) :
                (new[] {{ JsonConvert.DeserializeObject<string>(json) }});

            var {3} = await _repo.Get(ids, pageListParams);
            Response.AddPagination({3}.CurrentPage, {3}.PageSize,
                {3}.TotalCount, {3}.TotalPages);

            return Ok({3});
        }}


        [HttpGet(""{{id}}"", Name = ""Get{1}"")]
        public async Task<IActionResult> Get{1}(string id,  [FromQuery]SingleObjectParam param = null)
        {{
            if (!CheckAuthorization(""{4} - GetOne"", id, out string message))
                return Unauthorized(message);

            return Ok(await _repo.GetReturnObject(id, param));
        }}

        [HttpPost]
        public async Task<IActionResult> Add{1}([FromBody] object model)
        {{

            var json = model?.ToString();
            if (string.IsNullOrWhiteSpace(json))
            {{
                throw new InvalidDataException(""Please data you send is not valid; i.e empty"");
            }}
            var {3}ToAdd = json.IsJsonArrray() ?
                JsonConvert.DeserializeObject<{1}FromClientDto[]>(json) :
                (new[] {{ JsonConvert.DeserializeObject<{1}FromClientDto>(json) }});

            if (!CheckAuthorization(""{4} - Add"", {3}ToAdd, out string message))
                return Unauthorized(message);

            global::System.Console.WriteLine(JsonConvert.SerializeObject({3}ToAdd));

            for (int i = 0; i <{3}ToAdd?.Length; i++)
                {3}ToAdd[i].Id = $""{{i.ToString()}}000000090"";

            var {3} = this.Mapper.Map<Models.{0}.{1}[]>({3}ToAdd);
            await _repo.Add({3});

            if (ReturnType == ApiReturnTypes.NoContent)
                return NoContent();
            else if (ReturnType == ApiReturnTypes.FullObject)
                return Ok (FormatReturnObject ({3}, typeof ({1}ToClientDto)));
            else
                return Ok({3}.Select(x => x?.Id));
        }}

        [HttpPut(""{{id}}"")]
        public async Task<IActionResult> Update{1}(string id, [FromBody] {1}FromClientDto {3}ToUpdate)
        {{
            if (!CheckAuthorization(""{4} - Put"", {3}ToUpdate, out string message))
                return Unauthorized(message);

            var {3} = this.Mapper.Map<Models.{0}.{1}>({3}ToUpdate);
            await _repo.Update({3}, id);
            if (ReturnType == ApiReturnTypes.NoContent)
                return NoContent();
            else if (ReturnType == ApiReturnTypes.FullObject)
                return Ok (FormatReturnObject ({3}, typeof ({1}ToClientDto)));
            else
                return Ok({3}?.Id);
        }}

        [HttpPut]
        public async Task<IActionResult> Update{1}([FromBody] object models)
        {{
            var json = models?.ToString();
            var {3}ToUpdate = json.IsJsonArrray() ?
                JsonConvert.DeserializeObject<PutObject[]>(json) :
                (new[] {{ JsonConvert.DeserializeObject<PutObject>(json) }});

            if (!CheckAuthorization(""{4} - Put"", {3}ToUpdate, out string message))
                return Unauthorized(message);

            var {3} = await _repo.Update({3}ToUpdate);
			if ({3} == null)
            {{
                return NotFound(new
                {{
                    ErrorCode = 503,
                    ErrorMessage = ""Invalid Id: {5} id submitted could not be found""
                }});
            }}
			
            if (ReturnType == ApiReturnTypes.NoContent)
                return NoContent();
            else if (ReturnType == ApiReturnTypes.FullObject)
                 return Ok (FormatReturnObject ({3}, typeof ({1}ToClientDto)));
            else
                return Ok({3}?.Select(x => x.Id));
        }}

        [HttpPatch(""{{id}}"")]
        [TypeFilter(typeof(PatchBodyFilter))]
        public async Task<IActionResult> Patch{1}(string id, [FromBody] JsonPatchDocument patchDoc)
        {{
            if (!CheckAuthorization(""{4} - Patch"", patchDoc, out string message))
                return Unauthorized(message);

            var obj = await _repo.Update(patchDoc, id);
            if (obj == null)
            {{
                return NotFound(new
                {{
                    ErrorCode = 503,
                    ErrorMessage = ""Invalid Id: {5} id submitted could not be found""
                }});
            }}
            if (ReturnType == ApiReturnTypes.NoContent)
                return NoContent();
            else if (ReturnType == ApiReturnTypes.FullObject)
                 return Ok (FormatReturnObject (obj, typeof ({1}ToClientDto)));
            else
                return Ok(obj?.Id);
        }}

        [HttpPatch]
        [TypeFilter(typeof(PatchBodyFilter))]
        public async Task<IActionResult> Patch{2}([FromBody] object models)
        {{
            var json = models?.ToString();
            var patchDocs = json.IsJsonArrray() ?
                JsonConvert.DeserializeObject<PatchObject[]>(json) :
                (new[] {{ JsonConvert.DeserializeObject<PatchObject>(json) }});

            if (!CheckAuthorization(""{4} - Patch"", patchDocs, out string message))
                return Unauthorized(message);

            var objs = await _repo.Update(patchDocs);
            if (objs == null)
            {{
                return NotFound(new
                {{
                    ErrorCode = 503,
                    ErrorMessage = ""Invalid Id: {5} id submitted could not be found""
                }});
            }}
            if (ReturnType == ApiReturnTypes.NoContent)
                return NoContent();
            else if (ReturnType == ApiReturnTypes.FullObject)
                 return Ok (FormatReturnObject (objs, typeof ({1}ToClientDto)));
            else
                return Ok(objs?.Select(x => x.Id));
        }}

        [HttpDelete]
        public async Task<IActionResult> Delete{1}([FromBody] object models)
        {{
            var json = models?.ToString();
            var deleteObjs = json.IsJsonArrray() ?
                JsonConvert.DeserializeObject<IdObject[]>(json) :
                (new[] {{ JsonConvert.DeserializeObject<IdObject>(json) }});

            var ids = deleteObjs.Select(x => x.Id.ToString()).ToArray();

            if (!CheckAuthorization(""{4} - Delete"", ids, out string message))
                return Unauthorized(message);

            CheckIfRefferences(ids);

            await _repo.Delete(ids);
            return NoContent();
        }}

        [HttpDelete(""{{id}}"")]
        public async Task<IActionResult> DeleteObject{1}(string id)
        {{
            if (!CheckAuthorization(""{4} - Put"", id, out string message))
                return Unauthorized(message);

            if (string.IsNullOrWhiteSpace(id))
                throw new global::System.Exception(""Id to be deleted can not be null, please provide the id(s) to delete"");

            var ids = JsonToArrray(id);
            CheckIfRefferences(ids);

            await _repo.Delete(ids);
            return NoContent();
        }}";

    }
}