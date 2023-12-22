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

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// Update an existing qr request item.
/// </summary>
/// <remarks>
/// Update an existing qr request item by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateQRRequestItemRequest, UpdateQRRequestItemResponse>
{
  private const string EndPointId = "ENP-1S7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateQRRequestItemRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update QR Request Item End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full QR Request Item";
      s.Description = "This endpoint is used to update  qr request item, making a full replacement of qr request item with a specifed valuse. A valid qr request item is required.";
      s.ExampleRequest = new UpdateQRRequestItemRequest { CashSaleNumber = "Cash Sale Number", CostCentreCode = string.Empty, HsCode = "Hs Code", HsName = "Hs Name", ItemCode = "Item Code", ItemName = "Item Name", Narration = "Narration", PercentageDiscount = 0, Quantity = 0, RequestID = string.Empty, SaleID = "1000", Time = DateTime.Now, TotalAmount = 0, UnitOfMeasure = "Unit Of Measure", UnitPrice = 0, VATAmount = 0, VATClass = "VAT Class" };
      s.ResponseExamples[200] = new UpdateQRRequestItemResponse(new QRRequestItemRecord("Cash Sale Number", string.Empty, "Hs Code", "Hs Name", "Item Code", "Item Name", "Narration", 0, 0, string.Empty, "1000", DateTime.Now, 0, "Unit Of Measure", 0, 0, "VAT Class", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateQRRequestItemRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SaleID))
    {
      AddError(request => request.SaleID, "The sale id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<QRRequestItemDTO, QRRequestItem>(CreateEndPointUser.GetEndPointUser(User), request.SaleID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the qr request item to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<QRRequestItemDTO, QRRequestItem>(CreateEndPointUser.GetEndPointUser(User), request.SaleID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateQRRequestItemResponse(new QRRequestItemRecord(obj.CashSaleNumber, obj.CostCentreCode, obj.HsCode, obj.HsName, obj.ItemCode, obj.ItemName, obj.Narration, obj.PercentageDiscount, obj.Quantity, obj.RequestID, obj.Id, obj.Time, obj.TotalAmount, obj.UnitOfMeasure, obj.UnitPrice, obj.VATAmount, obj.VATClass, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
