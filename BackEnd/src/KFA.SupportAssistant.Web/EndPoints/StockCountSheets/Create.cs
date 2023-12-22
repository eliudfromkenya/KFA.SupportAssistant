using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

/// <summary>
/// Create a new StockCountSheet
/// </summary>
/// <remarks>
/// Creates a new stock count sheet given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateStockCountSheetRequest, CreateStockCountSheetResponse>
{
  private const string EndPointId = "ENP-1W1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateStockCountSheetRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Stock Count Sheet End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new stock count sheet";
      s.Description = "This endpoint is used to create a new  stock count sheet. Here details of stock count sheet to be created is provided";
      s.ExampleRequest = new CreateStockCountSheetRequest { Actual = 0, AverageAgeMonths = 0, BatchKey = string.Empty, CountSheetDocumentId = string.Empty, CountSheetId = "1000", DocumentNumber = "Document Number", ItemCode = string.Empty, Narration = "Narration", QuantityOnHand = 0, QuantitySoldLast12Months = 0, SellingPrice = 0, StocksOver = 0, StocksShort = 0, UnitCostPrice = 0 };
      s.ResponseExamples[200] = new CreateStockCountSheetResponse(0, 0, string.Empty, 0, "1000", "Document Number", string.Empty, "Narration", 0, 0, 0, 0, 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateStockCountSheetRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<StockCountSheetDTO>();
    requestDTO.Id = request.CountSheetId;

    var result = await mediator.Send(new CreateModelCommand<StockCountSheetDTO, StockCountSheet>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is StockCountSheetDTO obj)
      {
        Response = new CreateStockCountSheetResponse(obj.Actual, obj.AverageAgeMonths, obj.BatchKey, obj.CountSheetDocumentId, obj.Id, obj.DocumentNumber, obj.ItemCode, obj.Narration, obj.QuantityOnHand, obj.QuantitySoldLast12Months, obj.SellingPrice, obj.StocksOver, obj.StocksShort, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
