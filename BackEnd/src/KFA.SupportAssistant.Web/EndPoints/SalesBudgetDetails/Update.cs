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

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// Update an existing sales budget detail.
/// </summary>
/// <remarks>
/// Update an existing sales budget detail by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateSalesBudgetDetailRequest, UpdateSalesBudgetDetailResponse>
{
  private const string EndPointId = "ENP-1U7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateSalesBudgetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Sales Budget Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Sales Budget Detail";
      s.Description = "This endpoint is used to update  sales budget detail, making a full replacement of sales budget detail with a specifed valuse. A valid sales budget detail is required.";
      s.ExampleRequest = new UpdateSalesBudgetDetailRequest { BatchKey = "Batch Key", ItemCode = "Item Code", Month = 0, Month01Quantity = 0, Month02Quantity = 0, Month03Quantity = 0, Month04Quantity = 0, Month05Quantity = 0, Month06Quantity = 0, Month07Quantity = 0, Month08Quantity = 0, Month09Quantity = 0, Month10Quantity = 0, Month11Quantity = 0, Month12Quantity = 0, Narration = "Narration", SalesBudgetDetailId = "1000", SellingPrice = 0, UnitCostPrice = 0 };
      s.ResponseExamples[200] = new UpdateSalesBudgetDetailResponse(new SalesBudgetDetailRecord("Batch Key", "Item Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", "1000", 0, 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateSalesBudgetDetailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SalesBudgetDetailId))
    {
      AddError(request => request.SalesBudgetDetailId, "The sales budget detail id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<SalesBudgetDetailDTO, SalesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.SalesBudgetDetailId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the sales budget detail to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<SalesBudgetDetailDTO, SalesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.SalesBudgetDetailId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateSalesBudgetDetailResponse(new SalesBudgetDetailRecord(obj.BatchKey, obj.ItemCode, obj.Month, obj.Month01Quantity, obj.Month02Quantity, obj.Month03Quantity, obj.Month04Quantity, obj.Month05Quantity, obj.Month06Quantity, obj.Month07Quantity, obj.Month08Quantity, obj.Month09Quantity, obj.Month10Quantity, obj.Month11Quantity, obj.Month12Quantity, obj.Narration, obj.Id, obj.SellingPrice, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
