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

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchStockItemRequest, StockItemRecord>
{
  private const string EndPointId = "ENP-1Y6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchStockItemRequest.Route));
    //RequestBinder(new PatchBinder<StockItemDTO, StockItem, PatchStockItemRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Stock Item End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a stock item";
      s.Description = "Used to update part of an existing stock item. A valid existing stock item is required.";
      s.ResponseExamples[200] = new StockItemRecord("Barcode", string.Empty, "1000", "Item Name", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchStockItemRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ItemCode))
    {
      AddError(request => request.ItemCode, "The stock item of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    StockItemDTO patchFunc(StockItemDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<StockItemDTO, StockItem>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<StockItemDTO, StockItem>(CreateEndPointUser.GetEndPointUser(User), request.ItemCode ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the stock item to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new StockItemRecord(obj.Barcode, obj.GroupId, obj.Id, obj.ItemName, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
