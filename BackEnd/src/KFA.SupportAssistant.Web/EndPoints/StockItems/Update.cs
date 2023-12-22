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

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// Update an existing stock item.
/// </summary>
/// <remarks>
/// Update an existing stock item by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateStockItemRequest, UpdateStockItemResponse>
{
  private const string EndPointId = "ENP-1Y7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateStockItemRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Stock Item End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Stock Item";
      s.Description = "This endpoint is used to update  stock item, making a full replacement of stock item with a specifed valuse. A valid stock item is required.";
      s.ExampleRequest = new UpdateStockItemRequest { Barcode = "Barcode", GroupId = string.Empty, ItemCode = "1000", ItemName = "Item Name", Narration = "Narration" };
      s.ResponseExamples[200] = new UpdateStockItemResponse(new StockItemRecord("Barcode", string.Empty, "1000", "Item Name", "Narration", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateStockItemRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ItemCode))
    {
      AddError(request => request.ItemCode, "The item code of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<StockItemDTO, StockItem>(CreateEndPointUser.GetEndPointUser(User), request.ItemCode ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the stock item to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<StockItemDTO, StockItem>(CreateEndPointUser.GetEndPointUser(User), request.ItemCode ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateStockItemResponse(new StockItemRecord(obj.Barcode, obj.GroupId, obj.Id, obj.ItemName, obj.Narration, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
