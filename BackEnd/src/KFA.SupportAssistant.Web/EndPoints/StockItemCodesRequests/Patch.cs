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

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchStockItemCodesRequestRequest, StockItemCodesRequestRecord>
{
  private const string EndPointId = "ENP-1X6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchStockItemCodesRequestRequest.Route));
    //RequestBinder(new PatchBinder<StockItemCodesRequestDTO, StockItemCodesRequest, PatchStockItemCodesRequestRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Stock Item Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a stock item codes request";
      s.Description = "Used to update part of an existing stock item codes request. A valid existing stock item codes request is required.";
      s.ResponseExamples[200] = new StockItemCodesRequestRecord("Attanded By", "Cost Centre Code", 0, "Description", "Distributor", "Item Code", "1000", "Narration", "Requesting User", 0, "Status", "Supplier", DateTime.Now,DateTime.Now, "Unit Of Measure", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchStockItemCodesRequestRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ItemCodeRequestID))
    {
      AddError(request => request.ItemCodeRequestID, "The stock item codes request of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    StockItemCodesRequestDTO patchFunc(StockItemCodesRequestDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<StockItemCodesRequestDTO, StockItemCodesRequest>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<StockItemCodesRequestDTO, StockItemCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.ItemCodeRequestID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the stock item codes request to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new StockItemCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.CostPrice, obj.Description, obj.Distributor, obj.ItemCode, obj.Id, obj.Narration, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.Supplier, obj.TimeAttended, obj.TimeOfRequest, obj.UnitOfMeasure, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
