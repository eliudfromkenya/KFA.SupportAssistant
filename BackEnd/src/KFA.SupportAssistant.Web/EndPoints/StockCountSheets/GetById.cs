using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// Get a stock count sheet by count sheet id.
/// </summary>
/// <remarks>
/// Takes count sheet id and returns a matching stock count sheet record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetStockCountSheetByIdRequest, StockCountSheetRecord>
{
  private const string EndPointId = "ENP-1W4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetStockCountSheetByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Stock Count Sheet End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets stock count sheet by specified count sheet id";
      s.Description = "This endpoint is used to retrieve stock count sheet with the provided count sheet id";
      s.ExampleRequest = new GetStockCountSheetByIdRequest { CountSheetId = "count sheet id to retrieve" };
      s.ResponseExamples[200] = new StockCountSheetRecord(0, 0, string.Empty, 0, "1000", "Document Number", string.Empty, "Narration", 0, 0, 0, 0, 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetStockCountSheetByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CountSheetId))
    {
      AddError(request => request.CountSheetId, "The count sheet id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<StockCountSheetDTO, StockCountSheet>(CreateEndPointUser.GetEndPointUser(User), request.CountSheetId ?? "");
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
      Response = new StockCountSheetRecord(obj.Actual, obj.AverageAgeMonths, obj.BatchKey, obj.CountSheetDocumentId, obj.Id, obj.DocumentNumber, obj.ItemCode, obj.Narration, obj.QuantityOnHand, obj.QuantitySoldLast12Months, obj.SellingPrice, obj.StocksOver, obj.StocksShort, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
