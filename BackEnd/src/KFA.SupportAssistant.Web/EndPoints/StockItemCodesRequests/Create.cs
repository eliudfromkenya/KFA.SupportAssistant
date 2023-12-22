using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// Create a new StockItemCodesRequest
/// </summary>
/// <remarks>
/// Creates a new stock item codes request given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateStockItemCodesRequestRequest, CreateStockItemCodesRequestResponse>
{
  private const string EndPointId = "ENP-1X1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateStockItemCodesRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Stock Item Codes Request End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new stock item codes request";
      s.Description = "This endpoint is used to create a new  stock item codes request. Here details of stock item codes request to be created is provided";
      s.ExampleRequest = new CreateStockItemCodesRequestRequest { AttandedBy = "Attanded By", CostCentreCode = "Cost Centre Code", CostPrice = 0, Description = "Description", Distributor = "Distributor", ItemCode = "Item Code", ItemCodeRequestID = "1000", Narration = "Narration", RequestingUser = "Requesting User", SellingPrice = 0, Status = "Status", Supplier = "Supplier", TimeAttended = "Time Attended", TimeOfRequest = "Time of Request", UnitOfMeasure = "Unit Of Measure" };
      s.ResponseExamples[200] = new CreateStockItemCodesRequestResponse("Attanded By", "Cost Centre Code", 0, "Description", "Distributor", "Item Code", "1000", "Narration", "Requesting User", 0, "Status", "Supplier", DateTime.Now, DateTime.Now, "Unit Of Measure", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateStockItemCodesRequestRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<StockItemCodesRequestDTO>();
    requestDTO.Id = request.ItemCodeRequestID;

    var result = await mediator.Send(new CreateModelCommand<StockItemCodesRequestDTO, StockItemCodesRequest>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is StockItemCodesRequestDTO obj)
      {
        Response = new CreateStockItemCodesRequestResponse(obj.AttandedBy, obj.CostCentreCode, obj.CostPrice, obj.Description, obj.Distributor, obj.ItemCode, obj.Id, obj.Narration, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.Supplier, obj.TimeAttended, obj.TimeOfRequest, obj.UnitOfMeasure, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
