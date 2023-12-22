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

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// Update an existing stock count sheet.
/// </summary>
/// <remarks>
/// Update an existing stock count sheet by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateStockCountSheetRequest, UpdateStockCountSheetResponse>
{
  private const string EndPointId = "ENP-1W7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateStockCountSheetRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Stock Count Sheet End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Stock Count Sheet";
      s.Description = "This endpoint is used to update  stock count sheet, making a full replacement of stock count sheet with a specifed valuse. A valid stock count sheet is required.";
      s.ExampleRequest = new UpdateStockCountSheetRequest { Actual = 0, AverageAgeMonths = 0, BatchKey = string.Empty, CountSheetDocumentId = string.Empty, CountSheetId = "1000", DocumentNumber = "Document Number", ItemCode = string.Empty, Narration = "Narration", QuantityOnHand = 0, QuantitySoldLast12Months = 0, SellingPrice = 0, StocksOver = 0, StocksShort = 0, UnitCostPrice = 0 };
      s.ResponseExamples[200] = new UpdateStockCountSheetResponse(new StockCountSheetRecord(0, 0, string.Empty, 0, "1000", "Document Number", string.Empty, "Narration", 0, 0, 0, 0, 0, 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateStockCountSheetRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CountSheetId))
    {
      AddError(request => request.CountSheetId, "The count sheet id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<StockCountSheetDTO, StockCountSheet>(CreateEndPointUser.GetEndPointUser(User), request.CountSheetId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the stock count sheet to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<StockCountSheetDTO, StockCountSheet>(CreateEndPointUser.GetEndPointUser(User), request.CountSheetId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateStockCountSheetResponse(new StockCountSheetRecord(obj.Actual, obj.AverageAgeMonths, obj.BatchKey, obj.CountSheetDocumentId, obj.Id, obj.DocumentNumber, obj.ItemCode, obj.Narration, obj.QuantityOnHand, obj.QuantitySoldLast12Months, obj.SellingPrice, obj.StocksOver, obj.StocksShort, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
