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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetsMaintainceAccessoryRequest, ComputerAssetsMaintainceAccessoryRecord>
{
  private const string EndPointId = "ENP-1B6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetsMaintainceAccessoryRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory, PatchComputerAssetsMaintainceAccessoryRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Assets Maintaince Accessory End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer assets maintaince accessory";
      s.Description = "Used to update part of an existing computer assets maintaince accessory. A valid existing computer assets maintaince accessory is required.";
      s.ResponseExamples[200] = new ComputerAssetsMaintainceAccessoryRecord("", "1000", 0, "", DateTime.Now, "Description", "Invoice Number", "", "Narration", "Quotation Number", 0, "Vendor Code", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetsMaintainceAccessoryRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AccessoryID))
    {
      AddError(request => request.AccessoryID, "The computer assets maintaince accessory of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetsMaintainceAccessoryDTO patchFunc(ComputerAssetsMaintainceAccessoryDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory>(CreateEndPointUser.GetEndPointUser(User), request.AccessoryID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer assets maintaince accessory to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetsMaintainceAccessoryRecord(obj.AccessoryGroupID, obj.Id, obj.Amount, obj.AssetDetailID, obj.DateOfAcquisition, obj.Description, obj.InvoiceNumber, obj.MaintainceID, obj.Narration, obj.QuotationNumber, obj.VATAmount, obj.VendorCode, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
