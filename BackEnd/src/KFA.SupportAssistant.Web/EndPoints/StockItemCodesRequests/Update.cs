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

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// Update an existing stock item codes request.
/// </summary>
/// <remarks>
/// Update an existing stock item codes request by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateStockItemCodesRequestRequest, UpdateStockItemCodesRequestResponse>
{
  private const string EndPointId = "ENP-1X7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateStockItemCodesRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Stock Item Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Stock Item Codes Request";
      s.Description = "This endpoint is used to update  stock item codes request, making a full replacement of stock item codes request with a specifed valuse. A valid stock item codes request is required.";
      s.ExampleRequest = new UpdateStockItemCodesRequestRequest { AttandedBy = "Attanded By", CostCentreCode = "Cost Centre Code", CostPrice = 0, Description = "Description", Distributor = "Distributor", ItemCode = "Item Code", ItemCodeRequestID = "1000", Narration = "Narration", RequestingUser = "Requesting User", SellingPrice = 0, Status = "Status", Supplier = "Supplier", TimeAttended = "Time Attended", TimeOfRequest = "Time of Request", UnitOfMeasure = "Unit Of Measure" };
      s.ResponseExamples[200] = new UpdateStockItemCodesRequestResponse(new StockItemCodesRequestRecord("Attanded By", "Cost Centre Code", 0, "Description", "Distributor", "Item Code", "1000", "Narration", "Requesting User", 0, "Status", "Supplier",DateTime.Now, DateTime.Now, "Unit Of Measure", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateStockItemCodesRequestRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ItemCodeRequestID))
    {
      AddError(request => request.ItemCodeRequestID, "The item code request id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<StockItemCodesRequestDTO, StockItemCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.ItemCodeRequestID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the stock item codes request to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<StockItemCodesRequestDTO, StockItemCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.ItemCodeRequestID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateStockItemCodesRequestResponse(new StockItemCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.CostPrice, obj.Description, obj.Distributor, obj.ItemCode, obj.Id, obj.Narration, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.Supplier, obj.TimeAttended, obj.TimeOfRequest, obj.UnitOfMeasure, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
