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

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchStockCountSheetRequest, StockCountSheetRecord>
{
  private const string EndPointId = "ENP-1W6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchStockCountSheetRequest.Route));
    //RequestBinder(new PatchBinder<StockCountSheetDTO, StockCountSheet, PatchStockCountSheetRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Stock Count Sheet End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a stock count sheet";
      s.Description = "Used to update part of an existing stock count sheet. A valid existing stock count sheet is required.";
      s.ResponseExamples[200] = new StockCountSheetRecord(0, 0, string.Empty, 0, "1000", "Document Number", string.Empty, "Narration", 0, 0, 0, 0, 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchStockCountSheetRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CountSheetId))
    {
      AddError(request => request.CountSheetId, "The stock count sheet of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    StockCountSheetDTO patchFunc(StockCountSheetDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<StockCountSheetDTO, StockCountSheet>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<StockCountSheetDTO, StockCountSheet>(CreateEndPointUser.GetEndPointUser(User), request.CountSheetId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the stock count sheet to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new StockCountSheetRecord(obj.Actual, obj.AverageAgeMonths, obj.BatchKey, obj.CountSheetDocumentId, obj.Id, obj.DocumentNumber, obj.ItemCode, obj.Narration, obj.QuantityOnHand, obj.QuantitySoldLast12Months, obj.SellingPrice, obj.StocksOver, obj.StocksShort, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
