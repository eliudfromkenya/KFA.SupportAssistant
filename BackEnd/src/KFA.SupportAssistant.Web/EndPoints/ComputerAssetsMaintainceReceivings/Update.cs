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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// Update an existing computer assets maintaince receiving.
/// </summary>
/// <remarks>
/// Update an existing computer assets maintaince receiving by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetsMaintainceReceivingRequest, UpdateComputerAssetsMaintainceReceivingResponse>
{
  private const string EndPointId = "ENP-1D7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetsMaintainceReceivingRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Assets Maintaince Receiving End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Assets Maintaince Receiving";
      s.Description = "This endpoint is used to update  computer assets maintaince receiving, making a full replacement of computer assets maintaince receiving with a specifed valuse. A valid computer assets maintaince receiving is required.";
      s.ExampleRequest = new UpdateComputerAssetsMaintainceReceivingRequest { AssetDetailID = "", BroughtBy = "Brought By", DateRecieved = DateTime.Now, Description = "Description", FromDepartmentCode = "From Department Code", Narration = "Narration", ReasonToSend = "Reason To Send", ReceiveID = "1000", RecievedBy = "Recieved By" };
      s.ResponseExamples[200] = new UpdateComputerAssetsMaintainceReceivingResponse (new ComputerAssetsMaintainceReceivingRecord("", "Brought By", DateTime.Now, "Description", "From Department Code", "Narration", "Reason To Send", "1000", "Recieved By", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetsMaintainceReceivingRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ReceiveID))
    {
      AddError(request => request.ReceiveID , "The receive id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving>(CreateEndPointUser.GetEndPointUser(User), request.ReceiveID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer assets maintaince receiving to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving>(CreateEndPointUser.GetEndPointUser(User), request.ReceiveID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetsMaintainceReceivingResponse(new ComputerAssetsMaintainceReceivingRecord(obj.AssetDetailID, obj.BroughtBy, obj.DateRecieved, obj.Description, obj.FromDepartmentCode, obj.Narration, obj.ReasonToSend, obj.Id, obj.RecievedBy, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
