using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// Get a command detail by command id.
/// </summary>
/// <remarks>
/// Takes command id and returns a matching command detail record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetCommandDetailByIdRequest, CommandDetailRecord>
{
  private const string EndPointId = "ENP-124";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetCommandDetailByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Command Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets command detail by specified command id";
      s.Description = "This endpoint is used to retrieve command detail with the provided command id";
      s.ExampleRequest = new GetCommandDetailByIdRequest { CommandId = "command id to retrieve" };
      s.ResponseExamples[200] = new CommandDetailRecord("Action", "Active State", "Category", "1000", "Command Name", "Command Text", 0, "Image Path", true, true, "Narration", "Shortcut Key", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetCommandDetailByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CommandId))
    {
      AddError(request => request.CommandId, "The command id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<CommandDetailDTO, CommandDetail>(CreateEndPointUser.GetEndPointUser(User), request.CommandId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new CommandDetailRecord(obj.Action, obj.ActiveState, obj.Category, obj.Id, obj.CommandName, obj.CommandText, obj.ImageId, obj.ImagePath, obj.IsEnabled, obj.IsPublished, obj.Narration, obj.ShortcutKey, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
