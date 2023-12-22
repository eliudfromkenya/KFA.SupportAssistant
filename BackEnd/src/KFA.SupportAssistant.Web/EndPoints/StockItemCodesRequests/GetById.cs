using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// Get a stock item codes request by item code request id.
/// </summary>
/// <remarks>
/// Takes item code request id and returns a matching stock item codes request record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetStockItemCodesRequestByIdRequest, StockItemCodesRequestRecord>
{
  private const string EndPointId = "ENP-1X4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetStockItemCodesRequestByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Stock Item Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets stock item codes request by specified item code request id";
      s.Description = "This endpoint is used to retrieve stock item codes request with the provided item code request id";
      s.ExampleRequest = new GetStockItemCodesRequestByIdRequest { ItemCodeRequestID = "item code request id to retrieve" };
      s.ResponseExamples[200] = new StockItemCodesRequestRecord("Attanded By", "Cost Centre Code", 0, "Description", "Distributor", "Item Code", "1000", "Narration", "Requesting User", 0, "Status", "Supplier", DateTime.Now,DateTime.Now, "Unit Of Measure", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetStockItemCodesRequestByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ItemCodeRequestID))
    {
      AddError(request => request.ItemCodeRequestID, "The item code request id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<StockItemCodesRequestDTO, StockItemCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.ItemCodeRequestID ?? "");
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
      Response = new StockItemCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.CostPrice, obj.Description, obj.Distributor, obj.ItemCode, obj.Id, obj.Narration, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.Supplier, obj.TimeAttended, obj.TimeOfRequest, obj.UnitOfMeasure, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
