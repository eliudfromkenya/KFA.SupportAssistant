using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetsMaintainceReceivingRequest, ComputerAssetsMaintainceReceivingRecord>
{
  private const string EndPointId = "ENP-1D6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetsMaintainceReceivingRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving, PatchComputerAssetsMaintainceReceivingRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Assets Maintaince Receiving End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer assets maintaince receiving";
      s.Description = "Used to update part of an existing computer assets maintaince receiving. A valid existing computer assets maintaince receiving is required.";
      s.ResponseExamples[200] = new ComputerAssetsMaintainceReceivingRecord("", "Brought By", DateTime.Now, "Description", "From Department Code", "Narration", "Reason To Send", "1000", "Recieved By", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetsMaintainceReceivingRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ReceiveID))
    {
      AddError(request => request.ReceiveID , "The computer assets maintaince receiving of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetsMaintainceReceivingDTO patchFunc(ComputerAssetsMaintainceReceivingDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving>(CreateEndPointUser.GetEndPointUser(User), request.ReceiveID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer assets maintaince receiving to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);      
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetsMaintainceReceivingRecord(obj.AssetDetailID, obj.BroughtBy, obj.DateRecieved, obj.Description, obj.FromDepartmentCode, obj.Narration, obj.ReasonToSend, obj.Id, obj.RecievedBy, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
