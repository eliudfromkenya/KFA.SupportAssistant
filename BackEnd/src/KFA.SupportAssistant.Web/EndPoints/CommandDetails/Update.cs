using Ardalis.Result;
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

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

/// <summary>
/// Update an existing command detail.
/// </summary>
/// <remarks>
/// Update an existing command detail by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateCommandDetailRequest, UpdateCommandDetailResponse>
{
  private const string EndPointId = "ENP-127";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateCommandDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Command Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Command Detail";
      s.Description = "This endpoint is used to update  command detail, making a full replacement of command detail with a specifed valuse. A valid command detail is required.";
      s.ExampleRequest = new UpdateCommandDetailRequest { Action = "Action", ActiveState = "Active State", Category = "Category", CommandId = "1000", CommandName = "Command Name", CommandText = "Command Text", ImageId = string.Empty, ImagePath = "Image Path", IsEnabled = true, IsPublished = true, Narration = "Narration", ShortcutKey = "Shortcut Key" };
      s.ResponseExamples[200] = new UpdateCommandDetailResponse(new CommandDetailRecord("Action", "Active State", "Category", "1000", "Command Name", "Command Text", 0, "Image Path", true, true, "Narration", "Shortcut Key", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateCommandDetailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CommandId))
    {
      AddError(request => request.CommandId, "The command id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<CommandDetailDTO, CommandDetail>(CreateEndPointUser.GetEndPointUser(User), request.CommandId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the command detail to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<CommandDetailDTO, CommandDetail>(CreateEndPointUser.GetEndPointUser(User), request.CommandId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateCommandDetailResponse(new CommandDetailRecord(obj.Action, obj.ActiveState, obj.Category, obj.Id, obj.CommandName, obj.CommandText, obj.ImageId, obj.ImagePath, obj.IsEnabled, obj.IsPublished, obj.Narration, obj.ShortcutKey, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
