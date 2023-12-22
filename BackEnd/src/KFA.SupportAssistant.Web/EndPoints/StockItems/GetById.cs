using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// Get a stock item by item code.
/// </summary>
/// <remarks>
/// Takes item code and returns a matching stock item record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetStockItemByIdRequest, StockItemRecord>
{
  private const string EndPointId = "ENP-1Y4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetStockItemByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Stock Item End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets stock item by specified item code";
      s.Description = "This endpoint is used to retrieve stock item with the provided item code";
      s.ExampleRequest = new GetStockItemByIdRequest { ItemCode = "item code to retrieve" };
      s.ResponseExamples[200] = new StockItemRecord("Barcode", string.Empty, "1000", "Item Name", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetStockItemByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ItemCode))
    {
      AddError(request => request.ItemCode, "The item code of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<StockItemDTO, StockItem>(CreateEndPointUser.GetEndPointUser(User), request.ItemCode ?? "");
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
      Response = new StockItemRecord(obj.Barcode, obj.GroupId, obj.Id, obj.ItemName, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
