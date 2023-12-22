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

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// List all stock count sheets by specified conditions
/// </summary>
/// <remarks>
/// List all stock count sheets - returns a StockCountSheetListResponse containing the stock count sheets.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, StockCountSheetListResponse>
{
  private const string EndPointId = "ENP-1W5";
  public const string Route = "/stock_count_sheets";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Stock Count Sheets List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of stock count sheets as specified";
      s.Description = "Returns all stock count sheets as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new StockCountSheetListResponse { StockCountSheets = [new StockCountSheetRecord(0, 0, string.Empty, 0, "1000", "Document Number", string.Empty, "Narration", 0, 0, 0, 0, 0, 0, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<StockCountSheetDTO, StockCountSheet>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new StockCountSheetListResponse
      {
        StockCountSheets = result.Value.Select(obj => new StockCountSheetRecord(obj.Actual, obj.AverageAgeMonths, obj.BatchKey, obj.CountSheetDocumentId, obj.Id, obj.DocumentNumber, obj.ItemCode, obj.Narration, obj.QuantityOnHand, obj.QuantitySoldLast12Months, obj.SellingPrice, obj.StocksOver, obj.StocksShort, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
