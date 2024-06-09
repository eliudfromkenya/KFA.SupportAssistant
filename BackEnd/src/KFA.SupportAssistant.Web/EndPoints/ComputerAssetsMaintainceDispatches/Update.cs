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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// Update an existing computer assets maintaince dispatch.
/// </summary>
/// <remarks>
/// Update an existing computer assets maintaince dispatch by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetsMaintainceDispatchRequest, UpdateComputerAssetsMaintainceDispatchResponse>
{
  private const string EndPointId = "ENP-1C7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetsMaintainceDispatchRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Assets Maintaince Dispatch End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Assets Maintaince Dispatch";
      s.Description = "This endpoint is used to update  computer assets maintaince dispatch, making a full replacement of computer assets maintaince dispatch with a specifed valuse. A valid computer assets maintaince dispatch is required.";
      s.ExampleRequest = new UpdateComputerAssetsMaintainceDispatchRequest { AssetDetailID = "", CollectedBy = "Collected By", DateSend = DateTime.Now, Description = "Description", DispatchID = "1000", Narration = "Narration", ReasonToSend = "Reason To Send", SentBy = "Sent By", ToDepartmentCode = "To Department Code" };
      s.ResponseExamples[200] = new UpdateComputerAssetsMaintainceDispatchResponse (new ComputerAssetsMaintainceDispatchRecord("", "Collected By", DateTime.Now, "Description", "1000", "Narration", "Reason To Send", "Sent By", "To Department Code", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetsMaintainceDispatchRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.DispatchID))
    {
      AddError(request => request.DispatchID , "The dispatch id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch>(CreateEndPointUser.GetEndPointUser(User), request.DispatchID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer assets maintaince dispatch to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch>(CreateEndPointUser.GetEndPointUser(User), request.DispatchID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetsMaintainceDispatchResponse(new ComputerAssetsMaintainceDispatchRecord(obj.AssetDetailID, obj.CollectedBy, obj.DateSend, obj.Description, obj.Id, obj.Narration, obj.ReasonToSend, obj.SentBy, obj.ToDepartmentCode, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
