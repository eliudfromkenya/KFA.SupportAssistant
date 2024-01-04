using Ardalis.Result;
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

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// List all command details by specified conditions
/// </summary>
/// <remarks>
/// List all command details - returns a CommandDetailListResponse containing the command details.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, CommandDetailListResponse>
{
  private const string EndPointId = "ENP-125";
  public const string Route = "/command_details";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Command Details List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of command details as specified";
      s.Description = "Returns all command details as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new CommandDetailListResponse { CommandDetails = [new CommandDetailRecord("Action", "Active State", "Category", "1000", "Command Name", "Command Text", 0, "Image Path", true, true, "Narration", "Shortcut Key", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<CommandDetailDTO, CommandDetail>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<CommandDetailDTO>>.Success(ans.Select(v => (CommandDetailDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new CommandDetailListResponse
      {
        CommandDetails = result.Value.Select(obj => new CommandDetailRecord(obj.Action, obj.ActiveState, obj.Category, obj.Id, obj.CommandName, obj.CommandText, obj.ImageId, obj.ImagePath, obj.IsEnabled, obj.IsPublished, obj.Narration, obj.ShortcutKey, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
