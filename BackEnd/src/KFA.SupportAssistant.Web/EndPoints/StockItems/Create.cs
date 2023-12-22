using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

/// <summary>
/// Create a new StockItem
/// </summary>
/// <remarks>
/// Creates a new stock item given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateStockItemRequest, CreateStockItemResponse>
{
  private const string EndPointId = "ENP-1Y1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateStockItemRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Stock Item End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new stock item";
      s.Description = "This endpoint is used to create a new  stock item. Here details of stock item to be created is provided";
      s.ExampleRequest = new CreateStockItemRequest { Barcode = "Barcode", GroupId = string.Empty, ItemCode = "1000", ItemName = "Item Name", Narration = "Narration" };
      s.ResponseExamples[200] = new CreateStockItemResponse("Barcode", string.Empty, "1000", "Item Name", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateStockItemRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<StockItemDTO>();
    requestDTO.Id = request.ItemCode;

    var result = await mediator.Send(new CreateModelCommand<StockItemDTO, StockItem>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is StockItemDTO obj)
      {
        Response = new CreateStockItemResponse(obj.Barcode, obj.GroupId, obj.Id, obj.ItemName, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
