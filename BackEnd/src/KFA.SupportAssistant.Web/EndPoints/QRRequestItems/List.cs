using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

/// <summary>
/// List all qr request items by specified conditions
/// </summary>
/// <remarks>
/// List all qr request items - returns a QRRequestItemListResponse containing the qr request items.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, QRRequestItemListResponse>
{
  private const string EndPointId = "ENP-1S5";
  public const string Route = "/qr_request_items";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get QR Request Items List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of qr request items as specified";
      s.Description = "Returns all qr request items as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new QRRequestItemListResponse { QRRequestItems = [new QRRequestItemRecord("Cash Sale Number", string.Empty, "Hs Code", "Hs Name", "Item Code", "Item Name", "Narration", 0, 0, string.Empty, "1000", DateTime.Now, 0, "Unit Of Measure", 0, 0, "VAT Class", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<QRRequestItemDTO, QRRequestItem>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new QRRequestItemListResponse
      {
        QRRequestItems = result.Value.Select(obj => new QRRequestItemRecord(obj.CashSaleNumber, obj.CostCentreCode, obj.HsCode, obj.HsName, obj.ItemCode, obj.ItemName, obj.Narration, obj.PercentageDiscount, obj.Quantity, obj.RequestID, obj.Id, obj.Time, obj.TotalAmount, obj.UnitOfMeasure, obj.UnitPrice, obj.VATAmount, obj.VATClass, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
