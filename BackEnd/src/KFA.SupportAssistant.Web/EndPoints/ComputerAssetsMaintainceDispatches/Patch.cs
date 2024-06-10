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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetsMaintainceDispatchRequest, ComputerAssetsMaintainceDispatchRecord>
{
  private const string EndPointId = "ENP-1C6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetsMaintainceDispatchRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch, PatchComputerAssetsMaintainceDispatchRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Assets Maintaince Dispatch End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer assets maintaince dispatch";
      s.Description = "Used to update part of an existing computer assets maintaince dispatch. A valid existing computer assets maintaince dispatch is required.";
      s.ResponseExamples[200] = new ComputerAssetsMaintainceDispatchRecord("", "Collected By", DateTime.Now, "Description", "1000", "Narration", "Reason To Send", "Sent By", "To Department Code", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetsMaintainceDispatchRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.DispatchID))
    {
      AddError(request => request.DispatchID, "The computer assets maintaince dispatch of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetsMaintainceDispatchDTO patchFunc(ComputerAssetsMaintainceDispatchDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch>(CreateEndPointUser.GetEndPointUser(User), request.DispatchID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer assets maintaince dispatch to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetsMaintainceDispatchRecord(obj.AssetDetailID, obj.CollectedBy, obj.DateSend, obj.Description, obj.Id, obj.Narration, obj.ReasonToSend, obj.SentBy, obj.ToDepartmentCode, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
