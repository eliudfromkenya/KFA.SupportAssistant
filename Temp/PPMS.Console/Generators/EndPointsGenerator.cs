using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using PPMS.Console.Models;

namespace PPMS.Console.Generators;
  public class EndPointsGenerator
  {
    static readonly Queue<string> numbers = new();

    static void GenerateNumbers()
    {
        List<string> nums = new();
        char num = '0';
        for (int i = 0; i < 10; i++)
            nums.Add(((char)(((int)num) + i)).ToString());
        num = 'A';
        for (int i = 0; i < 26; i++)
            nums.Add(((char)(((int)num) + i)).ToString());

        for (int i = 1; i < nums.Count; i++)
            for (int j = 0; j < nums.Count; j++)
                numbers.Enqueue($"{nums[i]}{nums[j]}");
        //numbers.Dequeue();
    }
    internal static void GenerateModels()
    {
        try
        {
            GenerateNumbers();
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

                    return (
                        Rel: x,
                        Master: master,
                        Foreign: foreign,
                        MasterColumn: db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId)?.ColumnName,
                        ForeignColumn: db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)?.ColumnName,
                        MasterTableColumn: db.Columns.FirstOrDefault(m => m.Id == x.MasterColumnId),
                        ForeignTableColumn: db.Columns.FirstOrDefault(m => m.Id == x.ForeignColumnId)
                    );
                }).Where(x => x.Master != null && x.Foreign != null).ToArray();
            var dbGroups = db.Groups.ToArray();
            var dbColumns = db.Columns.ToArray();

            var contextConfigs = new Dictionary<string, List<Tuple<string, string>>>();
            //int tableCount = 1;
            var sbForeign = new StringBuilder();


            var relColIds = allRels.SelectMany(x => new[] { x.Rel.ForeignColumnId, x.Rel.MasterColumnId }).Distinct().ToArray();

            StringBuilder endPointsAccessRights = new();

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

                var colIds = columns.Select(v => v.Id)
                    .Concat(columns.Select(v => v.ColumnName))
                    .Distinct().ToList();

                var name = Functions.MakeAllFirstLetterCapital(table.StrimLinedName, false);
                var singular = Functions.Singularize(name)?.Replace(" ", "");
                var tblName = $"tbl_{name?.Replace(" ", "_")?.ToLower()}";
                var tableRels = allRels.Where(r => colIds.Contains(r.MasterColumn) || colIds.Contains(r.ForeignColumn)).ToList();

                var endPointId = numbers.Dequeue();
               using var createTsk = Task.Run(() => GenerateCreateEndPoint(table, columns, name, singular, tableRels, endPointId));
                using var deleteTsk = Task.Run(() => GenerateDeleteEndPoint(table, columns, name, singular, tableRels, endPointId));
                using var dynamicTsk = Task.Run(() => GenerateDynamicGetEndPoint(table, columns, name, singular, tableRels, endPointId));
                using var getByIdTsk = Task.Run(() => GenerateGetByIdEndPoint(table, columns, name, singular, tableRels, endPointId));
                using var listTsk = Task.Run(() => GenerateListEndPoint(table, columns, name, singular, tableRels, endPointId));
                using var patchTsk = Task.Run(() => GeneratePatchEndPoint(table, columns, name, singular, tableRels, endPointId));
                using var updateTsk = Task.Run(() => GenerateUpdateEndPoint(table, columns, name, singular, tableRels, endPointId));

                endPointsAccessRights.Append($@"
      #region {table.OriginalName}
      new DefaultAccessRight {{  Id = ""ENP-{endPointId}1"", Name=""{table.OriginalName}"", Type=""Create"" , Rights =$""{{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}}"" }},
      new DefaultAccessRight {{ Id = ""ENP-{endPointId}2"", Name = ""{table.OriginalName}"", Type = ""Delete"", Rights = $""{{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}}"" }},
      new DefaultAccessRight {{ Id = ""ENP-{endPointId}3"", Name = ""{table.OriginalName}"", Type = ""Dynamic Get"", Rights = $""{{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}},{{UserRoleConstants.ROLE_MANAGER}}"" }},
      new DefaultAccessRight {{ Id = ""ENP-{endPointId}4"", Name = ""{table.OriginalName}"", Type = ""Get By Id"", Rights = $""{{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}},{{UserRoleConstants.ROLE_MANAGER}}"" }},
      new DefaultAccessRight {{ Id = ""ENP-{endPointId}5"", Name = ""{table.OriginalName}"", Type = ""Get Many"", Rights = $""{{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}},{{UserRoleConstants.ROLE_MANAGER}}"" }},
      new DefaultAccessRight {{ Id = ""ENP-{endPointId}6"", Name = ""{table.OriginalName}"", Type = ""Patch"", Rights = $""{{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}}"" }},
      new DefaultAccessRight {{ Id = ""ENP-{endPointId}7"", Name = ""{table.OriginalName}"", Type = ""Update"", Rights = $""{{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}}"" }},
      #endregion

");

                Task.WaitAll(createTsk, updateTsk, deleteTsk, dynamicTsk, getByIdTsk, listTsk, patchTsk);
            }

            var path = Path.Combine(Defaults.MainPath, "EndPoints");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var rightsBody = $@"using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Infrastructure.Data.Seeds;
internal static class EndPointsAccessRights
{{

  public static async Task<bool> Process(AppDbContext db)
  {{
    DefaultAccessRight[] groups = [
      {endPointsAccessRights}
    ];
    var existingIds = db.DefaultAccessRights
      .Where(c => groups.Select(c => c.Id)
      .ToArray().Contains(c.Id))
      .Select(m => m.Id).ToList();

    groups = groups.Where(c => !existingIds.Contains(c.Id)).ToArray();
    if (groups.Length != 0)
    {{
      await db.DefaultAccessRights.AddRangeAsync(groups);
      await db.SaveChangesAsync();
    }}
    return true;
  }}
}}
";
            File.WriteAllText(Path.Combine(path, $"EndPointsAccessRights.cs"), rightsBody);
        }
        catch (Exception ex)
        {
            global::System.Console.WriteLine(ex.ToString());
        }
    }

    static string GetColumnType(TableColumn col, string[] relIds)
    {
        if (col.IsPrimary || relIds.Contains(col.ColumnName) || relIds.Contains(col.Id))
            return col.IsNullable ? "string?" : "string?";
        return col.Type.EndsWith("?") ? col.Type : $"{col.Type}?";
    }


    private static void GenerateUpdateEndPoint(DatabaseTable table, TableColumn[] columns, string name, string singular, List<(TableRelation Rel, DatabaseTable Master, DatabaseTable Foreign, string MasterColumn, string ForeignColumn, TableColumn MasterTableColumn, TableColumn ForeignTableColumn)> tableRels, string endpointId)
    {
        var path = Path.Combine(Defaults.MainPath, "EndPoints", name.MakeName());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var relIds = tableRels.SelectMany(m => new[] { m.MasterColumn, m.ForeignColumn }).Distinct().ToArray();

        var primary = columns.FirstOrDefault(c => c.IsPrimary);

        #region record type
        var colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()}")
             .ToArray();
        #endregion

        #region Create Validator
        var validatorType = $@"using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class Update{singular?.MakeName()}Validator : Validator<Update{singular?.MakeName()}Request>
{{
  public Update{singular?.MakeName()}Validator()
  {{
     {string.Join("\r\n\r\n", columns.Where(c => !c.IsNullable || ((c.Type?.ToLower()?.Contains("string") ?? false) && c.Length > 0)).Select(s =>
        {
            StringBuilder ans = new();
            if (!s.IsNullable)
                ans.AppendLine($@"RuleFor(x => x.{s.StrimLinedName.MakeName()})
     .NotEmpty()
     .WithMessage(""{s.OriginalColumnName} is required."")");
            else ans.AppendLine($@"RuleFor(x => x.{s.StrimLinedName.MakeName()})");

            if (s.Length > 0)
            {
                ans.AppendLine($@"     .MinimumLength({Math.Min(s.Length / 5, 2)})
     .MaximumLength({s.Length})");
            }

            return $"{ans.ToString()?.Trim()};";
        }).ToArray())}             

    static bool checkIds(string? objectId, string? urlId)
    {{
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }}

    RuleFor(x => x.{primary.StrimLinedName.MakeName()})
      .Must((args, id) => checkIds(args.{primary.StrimLinedName.MakeName()}, id))
      .WithMessage(""Route and body Ids must match; cannot update (change) Id of an existing resource."");
  }}
}}
";
        File.WriteAllText(Path.Combine(path, $"Update.Update{singular?.MakeName()}Validator.cs"), validatorType);
        #endregion

        #region Create Request
        var colTypes2 = columns
             .Select(m => (m.IsNullable ? "  " : "  [Required]\r\n  ") + $"public {GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()} {{ get; set; }}")
             .ToArray();

        var recquestype = $@"using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public record Update{singular?.MakeName()}Request
{{
  public const string Route = ""/{name?.ToLower()?.Replace(" ", "_")}/{{{primary.StrimLinedName.MakeName().Camelize()}}}"";
{(string.Join("\r\n", colTypes2))}
}}
";
        File.WriteAllText(Path.Combine(path, $"Update.Update{singular?.MakeName()}Request.cs"), recquestype);
        #endregion


        #region response type
        var colTypes3 = columns
             .Select(m => $"  public {GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()} {{ get; }} = {m.StrimLinedName.Camelize()};")
             .ToArray();

        colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName().Camelize()}")
             .ToArray();

        var responseType = $@"namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public class Update{singular?.MakeName()}Response
{{
  public Update{singular?.MakeName()}Response({singular?.MakeName()}Record {singular?.MakeName().Camelize()})
  {{
    {singular?.MakeName()} = {singular?.MakeName().Camelize()};
  }}

  public {singular?.MakeName()}Record {singular?.MakeName()} {{ get; set; }}
}}
";
        File.WriteAllText(Path.Combine(path, $"Update.Update{singular?.MakeName()}Response.cs"), responseType);
        #endregion

        List<(string, string, string)> defaults = new();
        string[] numberTypes = { "short", "long", "double", "byte", "int", "decimal" };
        foreach (var column in columns.Distinct())
        {
            if (column.IsPrimary)
                defaults.Add(("\"1000\"", column.StrimLinedName.MakeName() + " = \"1000\"", $"obj.Id"));
            else if (column.Type.ToLower().Contains("string"))
                defaults.Add(($"\"{column.OriginalColumnName}\"", column.StrimLinedName.MakeName() + $" = \"{column.OriginalColumnName}\"", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("date"))
                defaults.Add(("DateTime.Now", column.StrimLinedName.MakeName() + $" = DateTime.Now", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("bool"))
                defaults.Add(($"true", column.StrimLinedName.MakeName() + $" = true", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("byte[]"))
                defaults.Add(("new byte[]{}", column.StrimLinedName.MakeName() + $" = new byte[]{{}}", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (numberTypes.Any(c => column.Type.ToLower().Contains(c)))
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.IsNullable)
                defaults.Add(($"null", column.StrimLinedName.MakeName() + $" = null", $"obj.{column.StrimLinedName.MakeName()}"));
            else
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
        }

        var create = $@"using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// Update an existing {singular?.Titleize().ToLower()}.
/// </summary>
/// <remarks>
/// Update an existing {singular?.Titleize().ToLower()} by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<Update{singular?.Titleize()?.MakeName()}Request, Update{singular?.Titleize()?.MakeName()}Response>
{{
  private const string EndPointId = ""ENP-{endpointId}7"";

  public override void Configure()
  {{
    Put(CoreFunctions.GetURL(Update{singular?.Titleize()?.MakeName()}Request.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName(""Update {singular?.Titleize()} End Point""));
    Summary(s =>
    {{
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $""[End Point - {{EndPointId}}] Update a full {singular?.Titleize()}"";
      s.Description = ""This endpoint is used to update  {singular.Titleize().ToLower()}, making a full replacement of {singular?.Titleize()?.ToLower()} with a specifed valuse. A valid {singular?.Titleize()?.ToLower()} is required."";
      s.ExampleRequest = new Update{singular?.Titleize()?.MakeName()}Request {{ {string.Join(", ", defaults.Select(n => n.Item2))} }};
      s.ResponseExamples[200] = new Update{singular?.Titleize()?.MakeName()}Response (new {singular?.MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item1))}, DateTime.Now, DateTime.Now));
    }});
  }}

  public override async Task HandleAsync(
    Update{singular?.Titleize()?.MakeName()}Request request,
    CancellationToken cancellationToken)
  {{
    if (string.IsNullOrWhiteSpace(request.{primary?.StrimLinedName?.MakeName()}))
    {{
      AddError(request => request.{primary?.StrimLinedName?.MakeName()} , ""The {primary.OriginalColumnName.ToLower()} of the record to be updated is required please"");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }}

    var command = new GetModelQuery<{singular?.Titleize()?.MakeName()}DTO, {singular?.Titleize()?.MakeName()}>(CreateEndPointUser.GetEndPointUser(User), request.{primary?.StrimLinedName?.MakeName()} ?? """");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {{
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }}

    if (resultObj.Status == ResultStatus.NotFound)
    {{
      AddError(""Can not find the {singular.Titleize().ToLower()} to update"");
      await SendNotFoundAsync(cancellationToken);
      return;
    }}

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<{singular?.Titleize()?.MakeName()}DTO, {singular?.Titleize()?.MakeName()}>(CreateEndPointUser.GetEndPointUser(User), request.{primary?.StrimLinedName?.MakeName()} ?? """", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {{
      await SendNotFoundAsync(cancellationToken);
      return;
    }}

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {{
      Response = new Update{singular?.Titleize()?.MakeName()}Response(new {singular?.Titleize()?.MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item3))}, obj.DateInserted___, obj.DateUpdated___));
      return;
    }}
  }}
}}";

        File.WriteAllText(Path.Combine(path, $"Update.cs"), create);
    }



    private static void GenerateCreateEndPoint(DatabaseTable table, TableColumn[] columns, string name, string singular, List<(TableRelation Rel, DatabaseTable Master, DatabaseTable Foreign, string MasterColumn, string ForeignColumn, TableColumn MasterTableColumn, TableColumn ForeignTableColumn)> tableRels, string endpointId)
    {
        var path = Path.Combine(Defaults.MainPath, "EndPoints", name.MakeName());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var relIds = tableRels.SelectMany(m => new[] { m.MasterColumn, m.ForeignColumn }).Distinct().ToArray();

        var primary = columns.FirstOrDefault(c => c.IsPrimary);

        #region record type
        var colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()}")
             .ToArray();

        var recType = $@"namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public record {singular?.MakeName()}Record({(string.Join(", ", colTypes))}, DateTime? DateInserted___, DateTime? DateUpdated___);";
        File.WriteAllText(Path.Combine(path, $"{singular?.MakeName()}Record.cs"), recType);
        #endregion

        #region Create Validator
        var validatorType = $@"using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class Create{singular?.MakeName()}Validator : Validator<Create{singular?.MakeName()}Request>
{{
  public Create{singular?.MakeName()}Validator()
  {{
     {string.Join("\r\n\r\n", columns.Where(c => !c.IsNullable || ((c.Type?.ToLower()?.Contains("string") ?? false) && c.Length > 0)).Select(s =>
        {
            StringBuilder ans = new();
            if (!s.IsNullable)
                ans.AppendLine($@"RuleFor(x => x.{s.StrimLinedName.MakeName()})
     .NotEmpty()
     .WithMessage(""{s.OriginalColumnName} is required."")");
            else ans.AppendLine($@"RuleFor(x => x.{s.StrimLinedName.MakeName()})");

            if (s.Length > 0)
            {
                ans.AppendLine($@"     .MinimumLength({Math.Min(s.Length/5, 2)})
     .MaximumLength({s.Length})");
            }

            return  $"{ans.ToString()?.Trim()};";
        }).ToArray())}             
  }}
}}";
        File.WriteAllText(Path.Combine(path, $"Create.Create{singular?.MakeName()}Validator.cs"), validatorType);
        #endregion

        #region Create Request
        var colTypes2 = columns
             .Select(m => (m.IsNullable? "  ": "  [Required]\r\n  ") + $"public {GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()} {{ get; set; }}" )
             .ToArray();

        var recquestype = $@"using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public class Create{singular?.MakeName()}Request
{{
  public const string Route = ""/{name?.ToLower()?.Replace(" ","_")}"";
{(string.Join("\r\n", colTypes2))}
}}
";
        File.WriteAllText(Path.Combine(path, $"Create.Create{singular?.MakeName()}Request.cs"), recquestype);
        #endregion


        #region response type
        var colTypes3 = columns
             .Select(m => $"  public {GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()} {{ get; }} = {m.StrimLinedName.Camelize()};")
             .ToArray();

        colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName().Camelize()}")
             .ToArray();

        var responseType = $@"namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public readonly struct Create{singular?.MakeName()}Response({string.Join(", ", colTypes)}, DateTime? dateInserted___, DateTime? dateUpdated___)
{{
{(string.Join("\r\n", colTypes3))}
  public DateTime? DateInserted___ {{ get; }} = dateInserted___;
  public DateTime? DateUpdated___ {{ get; }} = dateUpdated___;
}}
";
        File.WriteAllText(Path.Combine(path, $"Create.Create{singular?.MakeName()}Response.cs"), responseType);
        #endregion

        List<(string, string,string)> defaults = new();
        string[] numberTypes = { "short", "long", "double", "byte", "int", "decimal" };
        foreach ( var column in columns.Distinct())
        {
            if (column.IsPrimary)
                defaults.Add(("\"1000\"", column.StrimLinedName.MakeName() + " = \"1000\"", $"obj.Id"));
            else if (column.Type.ToLower().Contains("string"))
                defaults.Add(($"\"{column.OriginalColumnName}\"", column.StrimLinedName.MakeName() + $" = \"{column.OriginalColumnName}\"", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("date"))
                defaults.Add(("DateTime.Now", column.StrimLinedName.MakeName() + $" = DateTime.Now", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("bool"))
                defaults.Add(($"true", column.StrimLinedName.MakeName() + $" = true", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("byte[]"))
                defaults.Add(("new byte[]{}", column.StrimLinedName.MakeName() + $" = new byte[]{{}}", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (numberTypes.Any(c => column.Type.ToLower().Contains(c)))
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.IsNullable)
                defaults.Add(($"null", column.StrimLinedName.MakeName() + $" = null", $"obj.{column.StrimLinedName.MakeName()}"));
            else
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
        }

        var create =$@"using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// Create a new {singular?.Titleize()?.MakeName()}
/// </summary>
/// <remarks>
/// Creates a new {singular?.Titleize()?.ToLower()} given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<Create{singular?.MakeName()}Request, Create{singular?.MakeName()}Response>
{{
  private const string EndPointId = ""ENP-{endpointId}1"";

  public override void Configure()
  {{
    Post(CoreFunctions.GetURL(Create{singular?.MakeName()}Request.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName(""Add {singular?.Titleize()?.Titleize()} End Point""));

    Summary(s =>
    {{
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $""[End Point - {{EndPointId}}] Used to create a new {singular.Titleize().ToLower()}"";
      s.Description = ""This endpoint is used to create a new  {singular.Titleize().ToLower()}. Here details of {singular.Titleize().ToLower()} to be created is provided"";
      s.ExampleRequest = new Create{singular?.MakeName()}Request {{ {string.Join(", ", defaults.Select(n => n.Item2))} }};
      s.ResponseExamples[200] = new Create{singular?.MakeName()}Response({string.Join(", ", defaults.Select(n => n.Item1))}, DateTime.Now, DateTime.Now);
    }});
  }}

  public override async Task HandleAsync(
    Create{singular?.MakeName()}Request request,
    CancellationToken cancellationToken)
  {{
    var requestDTO = request.Adapt<{singular?.MakeName()}DTO>();
    requestDTO.Id = request.{primary?.StrimLinedName?.MakeName()};

    var result = await mediator.Send(new CreateModelCommand<{singular?.MakeName()}DTO, {singular?.MakeName()}>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {{
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }}

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {{
      if (result?.Value?.FirstOrDefault() is {singular?.MakeName()}DTO obj)
      {{
        Response = new Create{singular?.MakeName()}Response({string.Join(", ", defaults.Select(n => n.Item3))}, obj.DateInserted___, obj.DateUpdated___);
        return;
      }}
    }}
  }}
}}
";

        File.WriteAllText(Path.Combine(path, $"Create.cs"), create);
    }


    private static void GenerateDeleteEndPoint(DatabaseTable table, TableColumn[] columns, string name, string singular, List<(TableRelation Rel, DatabaseTable Master, DatabaseTable Foreign, string MasterColumn, string ForeignColumn, TableColumn MasterTableColumn, TableColumn ForeignTableColumn)> tableRels, string endpointId)
    {
        var path = Path.Combine(Defaults.MainPath, "EndPoints", name.MakeName());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var relIds = tableRels.SelectMany(m => new[] { m.MasterColumn, m.ForeignColumn }).Distinct().ToArray();

        var primary = columns.FirstOrDefault(c => c.IsPrimary);

        #region record type
        var colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()}")
             .ToArray();
        #endregion

        #region Create Validator
        var validatorType = $@"using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class Delete{singular.MakeName()}Validator : Validator<Delete{singular.MakeName()}Request>
{{
  public Delete{singular.MakeName()}Validator()
  {{
    RuleFor(x => x.{primary.StrimLinedName.MakeName()})
      .NotEmpty()
      .WithMessage(""The {primary.OriginalColumnName.ToLower()} to be deleted is required please."")
      .MinimumLength(2)
      .MaximumLength(30);
  }}
}}

";
        File.WriteAllText(Path.Combine(path, $"Delete.Delete{singular?.MakeName()}Validator.cs"), validatorType);
        #endregion


        #region Create Validator
        var requestType = $@"namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public record Delete{singular.MakeName()}Request
{{
  public const string Route = ""/{name?.ToLower()?.Replace(" ", "_")}/{{{primary.StrimLinedName.MakeName().Camelize()}}}"";
  public static string BuildRoute(string? {primary.StrimLinedName.MakeName().Camelize()}) => Route.Replace(""{{{primary.StrimLinedName.MakeName().Camelize()}}}"", {primary.StrimLinedName.MakeName().Camelize()});
  public string? {primary.StrimLinedName.MakeName()} {{ get; set; }}
}}
";
        File.WriteAllText(Path.Combine(path, $"Delete.Delete{singular?.MakeName()}Request.cs"), requestType);
        #endregion

        var delete = $@"using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// Delete a {singular.Titleize().ToLower()}.
/// </summary>
/// <remarks>
/// Delete a {singular.Titleize().ToLower()} by providing a valid {primary.OriginalColumnName.ToLower()}.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<Delete{singular.MakeName()}Request>
{{
  private const string EndPointId = ""ENP-{endpointId}2"";

  public override void Configure()
  {{
    Delete(CoreFunctions.GetURL(Delete{singular.MakeName()}Request.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName(""Delete {singular.Titleize()} End Point""));
    Summary(s =>
    {{
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $""[End Point - {{EndPointId}}] Delete {singular.Titleize().ToLower()}"";
      s.Description = ""This endpoint is used to delete {singular.Titleize().ToLower()} with specified (provided) {primary.OriginalColumnName.ToLower()}(s)"";
      s.ExampleRequest = new Delete{singular.Titleize().MakeName()}Request {{ {primary.StrimLinedName.MakeName()} = ""AAA-01,AAA-02"" }};
      s.ResponseExamples = new Dictionary<int, object> {{ {{ 204, new object() }} }};
    }});
  }}

  public override async Task HandleAsync(
    Delete{singular.Titleize().MakeName()}Request request,
    CancellationToken cancellationToken)
  {{
    if (string.IsNullOrWhiteSpace(request.{primary.StrimLinedName.MakeName()}))
    {{
      AddError(request => request.{primary.StrimLinedName.MakeName()}, ""The {primary.OriginalColumnName.ToLower()} of the record to be deleted is required please"");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }}

    var command = new DeleteModelCommand<{singular.Titleize().MakeName()}>(CreateEndPointUser.GetEndPointUser(User), request.{primary.StrimLinedName.MakeName()} ?? """");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {{
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }}

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound)
    {{
      await SendNotFoundAsync(cancellationToken);
      return;
    }}

    if (result.IsSuccess)
    {{
      await SendNoContentAsync(cancellationToken);
    }};
  }}
}}

";

        File.WriteAllText(Path.Combine(path, $"Delete.cs"), delete);
    }


    private static void GenerateGetByIdEndPoint(DatabaseTable table, TableColumn[] columns, string name, string singular, List<(TableRelation Rel, DatabaseTable Master, DatabaseTable Foreign, string MasterColumn, string ForeignColumn, TableColumn MasterTableColumn, TableColumn ForeignTableColumn)> tableRels, string endpointId)
    {
        var path = Path.Combine(Defaults.MainPath, "EndPoints", name.MakeName());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var relIds = tableRels.SelectMany(m => new[] { m.MasterColumn, m.ForeignColumn }).Distinct().ToArray();

        var primary = columns.FirstOrDefault(c => c.IsPrimary);

        #region record type
        var colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()}")
             .ToArray();
        #endregion

        #region Create Validator
        var validatorType = $@"using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class Get{singular.MakeName()}Validator : Validator<Get{singular.MakeName()}ByIdRequest>
{{
  public Get{singular.MakeName()}Validator()
  {{
    RuleFor(x => x.{primary.StrimLinedName.MakeName()})
      .NotEmpty()
      .WithMessage(""The {primary.OriginalColumnName.ToLower()} to be fetched is required please."")
      .MinimumLength(2)
      .MaximumLength(30);
  }}
}}
";
        File.WriteAllText(Path.Combine(path, $"GetById.Get{singular.MakeName()}Validator.cs"), validatorType);
        #endregion


        #region Create Validator
        var requestType = $@"namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public class Get{singular.MakeName()}ByIdRequest
{{
  public const string Route = ""/{name?.ToLower()?.Replace(" ", "_")}/{{{primary.StrimLinedName.MakeName().Camelize()}}}"";

  public static string BuildRoute(string? {primary.StrimLinedName.MakeName().Camelize()}) => Route.Replace(""{{{primary.StrimLinedName.MakeName().Camelize()}}}"", {primary.StrimLinedName.MakeName().Camelize()});

  public string? {primary.StrimLinedName.MakeName()} {{ get; set; }}
}}
";
        File.WriteAllText(Path.Combine(path, $"GetById.Get{singular.MakeName()}ByIdRequest.cs"), requestType);
        #endregion

        List<(string, string, string)> defaults = new();
        string[] numberTypes = { "short", "long", "double", "byte", "int", "decimal" };
        foreach (var column in columns.Distinct())
        {
            if (column.IsPrimary)
                defaults.Add(("\"1000\"", column.StrimLinedName.MakeName() + " = \"1000\"", $"obj.Id"));
            else if (column.Type.ToLower().Contains("string"))
                defaults.Add(($"\"{column.OriginalColumnName}\"", column.StrimLinedName.MakeName() + $" = \"{column.OriginalColumnName}\"", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("date"))
                defaults.Add(("DateTime.Now", column.StrimLinedName.MakeName() + $" = DateTime.Now", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("bool"))
                defaults.Add(($"true", column.StrimLinedName.MakeName() + $" = true", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("byte[]"))
                defaults.Add(("new byte[]{}", column.StrimLinedName.MakeName() + $" = new byte[]{{}}", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (numberTypes.Any(c => column.Type.ToLower().Contains(c)))
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.IsNullable)
                defaults.Add(($"null", column.StrimLinedName.MakeName() + $" = null", $"obj.{column.StrimLinedName.MakeName()}"));
            else
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
        }

        var delete = $@"
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// Get a {singular.Titleize().ToLower()} by {primary.OriginalColumnName.ToLower()}.
/// </summary>
/// <remarks>
/// Takes {primary.OriginalColumnName.ToLower()} and returns a matching {singular.Titleize().ToLower()} record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<Get{singular.Titleize().MakeName()}ByIdRequest, {singular.Titleize().MakeName()}Record>
{{
  private const string EndPointId = ""ENP-{endpointId}4"";

  public override void Configure()
  {{
    Get(CoreFunctions.GetURL(Get{singular.Titleize().MakeName()}ByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName(""Get {singular.Titleize()} End Point""));
    Summary(s =>
    {{
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $""[End Point - {{EndPointId}}] Gets {singular.Titleize() .ToLower()} by specified {primary.OriginalColumnName.ToLower()}"";
      s.Description = ""This endpoint is used to retrieve {singular.Titleize().ToLower()} with the provided {primary.OriginalColumnName.ToLower()}"";
      s.ExampleRequest = new Get{singular?.MakeName()}ByIdRequest {{ {primary.StrimLinedName.MakeName()} = ""{primary.OriginalColumnName.ToLower()} to retrieve"" }};
      s.ResponseExamples[200] = new {singular?.MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item1))}, DateTime.Now, DateTime.Now);
    }});
  }}

  public override async Task HandleAsync(Get{singular.Titleize().MakeName()}ByIdRequest request,
    CancellationToken cancellationToken)
  {{
    if (string.IsNullOrWhiteSpace(request.{primary.StrimLinedName.MakeName()}))
    {{
      AddError(request => request.{primary.StrimLinedName.MakeName()}, ""The {primary.OriginalColumnName.ToLower()} of the record to be retrieved is required please"");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }}

    var command = new GetModelQuery<{singular.Titleize().MakeName()}DTO, {singular.Titleize().MakeName()}>(CreateEndPointUser.GetEndPointUser(User), request.{primary.StrimLinedName.MakeName()} ?? """");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {{
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }}

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {{
      await SendNotFoundAsync(cancellationToken);
      return;
    }}
    var obj = result.Value;
    if (result.IsSuccess)
    {{
      Response = new {singular.Titleize().MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item3))}, obj.DateInserted___, obj.DateUpdated___);
      return;
    }}
  }}
}}
";

        File.WriteAllText(Path.Combine(path, $"GetById.cs"), delete);
    }


    private static void GenerateDynamicGetEndPoint(DatabaseTable table, TableColumn[] columns, string name, string singular, List<(TableRelation Rel, DatabaseTable Master, DatabaseTable Foreign, string MasterColumn, string ForeignColumn, TableColumn MasterTableColumn, TableColumn ForeignTableColumn)> tableRels, string endpointId)
    {
        var path = Path.Combine(Defaults.MainPath, "EndPoints", name.MakeName());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var relIds = tableRels.SelectMany(m => new[] { m.MasterColumn, m.ForeignColumn }).Distinct().ToArray();

        var primary = columns.FirstOrDefault(c => c.IsPrimary);

        #region record type
        var colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()}")
             .ToArray();
        #endregion


        var dynamicGet = $@"using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// List all {singular.Titleize().ToLower()}.
/// </summary>
/// <remarks>
/// Dynamically Get all {singular.Titleize().ToLower()} as specified - returns a dynamic list of the {singular.Titleize().ToLower()}.
/// </remarks>
public class DynamicGet(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, string>
{{
  private const string EndPointId = ""ENP-{endpointId}3"";
  public const string Route = ""/{name?.ToLower()?.Replace(" ", "_")}/dynamically"";

  public override void Configure()
  {{
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName(""Get {table.OriginalName.Titleize()} Dynamically End Point""));
    Summary(s =>
    {{
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $""[End Point - {{EndPointId}}] Retrieves dynamically of {table.OriginalName.ToLower()} as specified"";
      s.Description = ""Dynamically returns all {table.OriginalName.ToLower()} as specified, i.e filter to specify which records or rows to return, selector to specify which properties or columns to return, order to specify order criteria. It returns a JSON result in form of a string."";
      s.ExampleRequest = new ListParam {{ Param = JsonConvert.SerializeObject(new FilterParam {{ Predicate = ""Id.Trim().StartsWith(@0) and Id >= @1"", SelectColumns = ""new (Id, Narration)"", Parameters = [""SVR"", ""SVR-01""], OrderByConditions = [""Id"", ""Narration""] }}), Skip = 0, Take = 1000 }};
    }});
  }}

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {{
    var command = new DynamicsListModelsQuery<{singular.MakeName()}DTO, {singular.MakeName()}>(CreateEndPointUser.GetEndPointUser(User), request ?? new ListParam {{ }});
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {{
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }}

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {{
      Response = result.Value;
    }}
  }}
}}


";

        File.WriteAllText(Path.Combine(path, $"DynamicGet.cs"), dynamicGet);
    }

    private static void GenerateListEndPoint(DatabaseTable table, TableColumn[] columns, string name, string singular, List<(TableRelation Rel, DatabaseTable Master, DatabaseTable Foreign, string MasterColumn, string ForeignColumn, TableColumn MasterTableColumn, TableColumn ForeignTableColumn)> tableRels, string endpointId)
    {
        var path = Path.Combine(Defaults.MainPath, "EndPoints", name.MakeName());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var relIds = tableRels.SelectMany(m => new[] { m.MasterColumn, m.ForeignColumn }).Distinct().ToArray();

        var primary = columns.FirstOrDefault(c => c.IsPrimary);

        #region record type
        var colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()}")
             .ToArray();
        #endregion

        #region Create Validator
        var validatorType = $@"namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public class {singular.MakeName()}ListResponse
{{
  public List<{singular.MakeName()}Record> {table.StrimLinedName.MakeName()} {{ get; set; }} = [];
}}
";
        File.WriteAllText(Path.Combine(path, $"List.{singular.MakeName()}ListResponse.cs"), validatorType);
        #endregion

        List<(string, string, string)> defaults = new();
        string[] numberTypes = { "short", "long", "double", "byte", "int", "decimal" };
        foreach (var column in columns.Distinct())
        {
            if (column.IsPrimary)
                defaults.Add(("\"1000\"", column.StrimLinedName.MakeName() + " = \"1000\"", $"obj.Id"));
            else if (column.Type.ToLower().Contains("string"))
                defaults.Add(($"\"{column.OriginalColumnName}\"", column.StrimLinedName.MakeName() + $" = \"{column.OriginalColumnName}\"", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("date"))
                defaults.Add(("DateTime.Now", column.StrimLinedName.MakeName() + $" = DateTime.Now", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("bool"))
                defaults.Add(($"true", column.StrimLinedName.MakeName() + $" = true", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("byte[]"))
                defaults.Add(("new byte[]{}", column.StrimLinedName.MakeName() + $" = new byte[]{{}}", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (numberTypes.Any(c => column.Type.ToLower().Contains(c)))
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.IsNullable)
                defaults.Add(($"null", column.StrimLinedName.MakeName() + $" = null", $"obj.{column.StrimLinedName.MakeName()}"));
            else
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
        }

        var list = $@"
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// List all {table.OriginalName.ToLower()} by specified conditions
/// </summary>
/// <remarks>
/// List all {table.OriginalName.ToLower()} - returns a {singular.MakeName()}ListResponse containing the {table.OriginalName.ToLower()}.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, {singular.MakeName()}ListResponse>
{{
  private const string EndPointId = ""ENP-{endpointId}5"";
  public const string Route = ""/{name?.ToLower()?.Replace(" ", "_")}"";

  public override void Configure()
  {{
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName(""Get {table.OriginalName.Titleize()} List End Point""));

    Summary(s =>
    {{
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $""[End Point - {{EndPointId}}] Retrieves list of {table.OriginalName.ToLower()} as specified"";
      s.Description = ""Returns all {table.OriginalName.ToLower()} as specified, i.e filter to specify which records or rows to return, order to specify order criteria"";
      s.ResponseExamples[200] = new {singular.MakeName()}ListResponse {{ {table.StrimLinedName.MakeName()} = [new {singular?.MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item1))}, DateTime.Now, DateTime.Now)] }};
      s.ExampleRequest = new ListParam {{ Param = JsonConvert.SerializeObject(new FilterParam {{ Predicate = ""Id.Trim().StartsWith(@0) and Id >= @1"", SelectColumns = ""new {{Id, Narration}}"", Parameters = [""S3"", ""3100""], OrderByConditions = [""Id"", ""Narration""] }}), Skip = 0, Take = 1000 }};
    }});
  }}

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {{
    var command = new ListModelsQuery<{singular.MakeName()}DTO, {singular.MakeName()}>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {{
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }}

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {{
      Response = new {singular.MakeName()}ListResponse
      {{
        {table.StrimLinedName.MakeName()} = result.Value.Select(obj => new {singular.Titleize().MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item3))}, obj.DateInserted___, obj.DateUpdated___)).ToList()
      }};
    }}
  }}
}}
";
        File.WriteAllText(Path.Combine(path, $"List.cs"), list);
    }



    private static void GeneratePatchEndPoint(DatabaseTable table, TableColumn[] columns, string name, string singular, List<(TableRelation Rel, DatabaseTable Master, DatabaseTable Foreign, string MasterColumn, string ForeignColumn, TableColumn MasterTableColumn, TableColumn ForeignTableColumn)> tableRels, string endpointId)
    {
        var path = Path.Combine(Defaults.MainPath, "EndPoints", name.MakeName());
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var relIds = tableRels.SelectMany(m => new[] { m.MasterColumn, m.ForeignColumn }).Distinct().ToArray();

        var primary = columns.FirstOrDefault(c => c.IsPrimary);

        #region record type
        var colTypes = columns
             .Select(m => $"{GetColumnType(m, relIds)} {m.StrimLinedName.MakeName()}")
             .ToArray();
        #endregion

        #region Create Validator
        var validatorType = $@"using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class Patch{singular.MakeName()}Validator : Validator<Patch{singular.MakeName()}Request>
{{
  public Patch{singular.MakeName()}Validator()
  {{
    RuleFor(x => x.{primary.StrimLinedName.MakeName()})
     .NotEmpty()
     .WithMessage(""The {primary.OriginalColumnName.ToLower()} of the record to be updated is required"")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage(""Body or content to update is required."");
  }}
}}

";
        File.WriteAllText(Path.Combine(path, $"Patch.Patch{singular.MakeName()}Validator.cs"), validatorType);
        #endregion


        #region Create Validator
        var requestType = $@"using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public class Patch{singular.MakeName()}Request : JsonPatchDocument<{singular.MakeName()}DTO>, IPlainTextRequest
{{
  public const string Route = ""/{name?.ToLower()?.Replace(" ", "_")}/{{{primary.StrimLinedName.MakeName().Camelize()}}}"";

  public static string BuildRoute(string {primary.StrimLinedName.MakeName().Camelize()}) => Route.Replace(""{{{primary.StrimLinedName.MakeName().Camelize()}}}"", {primary.StrimLinedName.MakeName().Camelize()});

  public string {primary.StrimLinedName.MakeName()} {{ get; set; }} = string.Empty;
  public string Content {{ get; set; }} = string.Empty;

  public JsonPatchDocument<{singular.MakeName()}DTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<{singular.MakeName()}DTO>>(Content)!;
}}
";
        File.WriteAllText(Path.Combine(path, $"Patch.Patch{singular.MakeName()}Request.cs"), requestType);
        #endregion


        List<(string, string, string)> defaults = new();
        string[] numberTypes = { "short", "long", "double", "byte", "int", "decimal" };
        foreach (var column in columns.Distinct())
        {
            if (column.IsPrimary)
                defaults.Add(("\"1000\"", column.StrimLinedName.MakeName() + " = \"1000\"", $"obj.Id"));
            else if (column.Type.ToLower().Contains("string"))
                defaults.Add(($"\"{column.OriginalColumnName}\"", column.StrimLinedName.MakeName() + $" = \"{column.OriginalColumnName}\"", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("date"))
                defaults.Add(("DateTime.Now", column.StrimLinedName.MakeName() + $" = DateTime.Now", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("bool"))
                defaults.Add(($"true", column.StrimLinedName.MakeName() + $" = true", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.Type.ToLower().Contains("byte[]"))
                defaults.Add(("new byte[]{}", column.StrimLinedName.MakeName() + $" = new byte[]{{}}", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (numberTypes.Any(c => column.Type.ToLower().Contains(c)))
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
            else if (column.IsNullable)
                defaults.Add(($"null", column.StrimLinedName.MakeName() + $" = null", $"obj.{column.StrimLinedName.MakeName()}"));
            else
                defaults.Add(($"0", column.StrimLinedName.MakeName() + $" = 0", $"obj.{column.StrimLinedName.MakeName()}"));
        }


        var patch = $@"using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.{name.MakeName()};

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<Patch{singular.MakeName()}Request, {singular.MakeName()}Record>
{{
  private const string EndPointId = ""ENP-{endpointId}6"";

  public override void Configure()
  {{
    Patch(CoreFunctions.GetURL(Patch{singular.MakeName()}Request.Route));
    //RequestBinder(new PatchBinder<{singular.MakeName()}DTO, {singular.MakeName()}, Patch{singular.MakeName()}Request>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName(""Partial Update {singular.Titleize()} End Point""));
    Summary(s =>
    {{
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $""[End Point - {{EndPointId}}] Update partially a {singular.Titleize().ToLower()}"";
      s.Description = ""Used to update part of an existing {singular.Titleize().ToLower()}. A valid existing {singular.Titleize().ToLower()} is required."";
      s.ResponseExamples[200] = new {singular?.MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item1))}, DateTime.Now, DateTime.Now);
    }});
  }}

  public override async Task HandleAsync(Patch{singular.MakeName()}Request request, CancellationToken cancellationToken)
  {{
    if (string.IsNullOrWhiteSpace(request.{primary.StrimLinedName.MakeName()}))
    {{
      AddError(request => request.{primary.StrimLinedName.MakeName()} , ""The {singular.Titleize().ToLower()} of the record to be updated is required please"");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }}

    {singular.MakeName()}DTO patchFunc({singular.MakeName()}DTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<{singular.MakeName()}DTO, {singular.MakeName()}>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<{singular.MakeName()}DTO, {singular.MakeName()}>(CreateEndPointUser.GetEndPointUser(User), request.{primary.StrimLinedName.MakeName()} ?? """", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError(""Can not find the {singular.Titleize().ToLower()} to update"");

    if (result.Errors.Any())
    {{
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);      
    }}

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {{
      Response = new {singular.Titleize().MakeName()}Record({string.Join(", ", defaults.Select(n => n.Item3))}, obj.DateInserted___, obj.DateUpdated___);
    }}
  }}
}}
";
        File.WriteAllText(Path.Combine(path, $"Patch.cs"), patch);
    }

}

