using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// Get a qr request item by sale id.
/// </summary>
/// <remarks>
/// Takes sale id and returns a matching qr request item record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetQRRequestItemByIdRequest, QRRequestItemRecord>
{
  private const string EndPointId = "ENP-1S4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetQRRequestItemByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get QR Request Item End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets qr request item by specified sale id";
      s.Description = "This endpoint is used to retrieve qr request item with the provided sale id";
      s.ExampleRequest = new GetQRRequestItemByIdRequest { SaleID = "sale id to retrieve" };
      s.ResponseExamples[200] = new QRRequestItemRecord("Cash Sale Number", string.Empty, "Hs Code", "Hs Name", "Item Code", "Item Name", "Narration", 0, 0, string.Empty, "1000", DateTime.Now, 0, "Unit Of Measure", 0, 0, "VAT Class", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetQRRequestItemByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SaleID))
    {
      AddError(request => request.SaleID, "The sale id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<QRRequestItemDTO, QRRequestItem>(CreateEndPointUser.GetEndPointUser(User), request.SaleID ?? "");
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
      Response = new QRRequestItemRecord(obj.CashSaleNumber, obj.CostCentreCode, obj.HsCode, obj.HsName, obj.ItemCode, obj.ItemName, obj.Narration, obj.PercentageDiscount, obj.Quantity, obj.RequestID, obj.Id, obj.Time, obj.TotalAmount, obj.UnitOfMeasure, obj.UnitPrice, obj.VATAmount, obj.VATClass, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
