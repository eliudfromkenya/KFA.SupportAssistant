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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetTransferRequest, ComputerAssetTransferRecord>
{
  private const string EndPointId = "ENP-196";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetTransferRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetTransferDTO, ComputerAssetTransfer, PatchComputerAssetTransferRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Asset Transfer End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer asset transfer";
      s.Description = "Used to update part of an existing computer asset transfer. A valid existing computer asset transfer is required.";
      s.ResponseExamples[200] = new ComputerAssetTransferRecord("", "Cost Centre Code", "Narration", "Payroll Number", "Responsible User", "Status", DateTime.Now, "1000", "Transfer Reasons", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetTransferRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.TransferID))
    {
      AddError(request => request.TransferID , "The computer asset transfer of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetTransferDTO patchFunc(ComputerAssetTransferDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetTransferDTO, ComputerAssetTransfer>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetTransferDTO, ComputerAssetTransfer>(CreateEndPointUser.GetEndPointUser(User), request.TransferID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer asset transfer to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);      
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetTransferRecord(obj.AssetDetailID, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.ResponsibleUser, obj.Status, obj.TransferDate, obj.Id, obj.TransferReasons, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
