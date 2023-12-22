using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// Create a new QRRequestItem
/// </summary>
/// <remarks>
/// Creates a new qr request item given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateQRRequestItemRequest, CreateQRRequestItemResponse>
{
  private const string EndPointId = "ENP-1S1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateQRRequestItemRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add QR Request Item End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new qr request item";
      s.Description = "This endpoint is used to create a new  qr request item. Here details of qr request item to be created is provided";
      s.ExampleRequest = new CreateQRRequestItemRequest { CashSaleNumber = "Cash Sale Number", CostCentreCode = string.Empty, HsCode = "Hs Code", HsName = "Hs Name", ItemCode = "Item Code", ItemName = "Item Name", Narration = "Narration", PercentageDiscount = 0, Quantity = 0, RequestID = string.Empty, SaleID = "1000", Time = DateTime.Now, TotalAmount = 0, UnitOfMeasure = "Unit Of Measure", UnitPrice = 0, VATAmount = 0, VATClass = "VAT Class" };
      s.ResponseExamples[200] = new CreateQRRequestItemResponse("Cash Sale Number", string.Empty, "Hs Code", "Hs Name", "Item Code", "Item Name", "Narration", 0, 0, string.Empty, "1000", DateTime.Now, 0, "Unit Of Measure", 0, 0, "VAT Class", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateQRRequestItemRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<QRRequestItemDTO>();
    requestDTO.Id = request.SaleID;

    var result = await mediator.Send(new CreateModelCommand<QRRequestItemDTO, QRRequestItem>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is QRRequestItemDTO obj)
      {
        Response = new CreateQRRequestItemResponse(obj.CashSaleNumber, obj.CostCentreCode, obj.HsCode, obj.HsName, obj.ItemCode, obj.ItemName, obj.Narration, obj.PercentageDiscount, obj.Quantity, obj.RequestID, obj.Id, obj.Time, obj.TotalAmount, obj.UnitOfMeasure, obj.UnitPrice, obj.VATAmount, obj.VATClass, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
