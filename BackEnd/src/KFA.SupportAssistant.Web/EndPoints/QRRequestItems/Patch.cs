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

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchQRRequestItemRequest, QRRequestItemRecord>
{
  private const string EndPointId = "ENP-1S6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchQRRequestItemRequest.Route));
    //RequestBinder(new PatchBinder<QRRequestItemDTO, QRRequestItem, PatchQRRequestItemRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update QR Request Item End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a qr request item";
      s.Description = "Used to update part of an existing qr request item. A valid existing qr request item is required.";
      s.ResponseExamples[200] = new QRRequestItemRecord("Cash Sale Number", string.Empty, "Hs Code", "Hs Name", "Item Code", "Item Name", "Narration", 0, 0, string.Empty, "1000", DateTime.Now, 0, "Unit Of Measure", 0, 0, "VAT Class", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchQRRequestItemRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SaleID))
    {
      AddError(request => request.SaleID, "The qr request item of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    QRRequestItemDTO patchFunc(QRRequestItemDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<QRRequestItemDTO, QRRequestItem>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<QRRequestItemDTO, QRRequestItem>(CreateEndPointUser.GetEndPointUser(User), request.SaleID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the qr request item to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new QRRequestItemRecord(obj.CashSaleNumber, obj.CostCentreCode, obj.HsCode, obj.HsName, obj.ItemCode, obj.ItemName, obj.Narration, obj.PercentageDiscount, obj.Quantity, obj.RequestID, obj.Id, obj.Time, obj.TotalAmount, obj.UnitOfMeasure, obj.UnitPrice, obj.VATAmount, obj.VATClass, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
