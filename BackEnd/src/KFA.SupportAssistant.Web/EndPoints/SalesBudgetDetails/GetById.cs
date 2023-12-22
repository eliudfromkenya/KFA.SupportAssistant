using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

/// <summary>
/// Get a sales budget detail by sales budget detail id.
/// </summary>
/// <remarks>
/// Takes sales budget detail id and returns a matching sales budget detail record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetSalesBudgetDetailByIdRequest, SalesBudgetDetailRecord>
{
  private const string EndPointId = "ENP-1U4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetSalesBudgetDetailByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Sales Budget Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets sales budget detail by specified sales budget detail id";
      s.Description = "This endpoint is used to retrieve sales budget detail with the provided sales budget detail id";
      s.ExampleRequest = new GetSalesBudgetDetailByIdRequest { SalesBudgetDetailId = "sales budget detail id to retrieve" };
      s.ResponseExamples[200] = new SalesBudgetDetailRecord("Batch Key", "Item Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", "1000", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetSalesBudgetDetailByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SalesBudgetDetailId))
    {
      AddError(request => request.SalesBudgetDetailId, "The sales budget detail id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<SalesBudgetDetailDTO, SalesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.SalesBudgetDetailId ?? "");
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
      Response = new SalesBudgetDetailRecord(obj.BatchKey, obj.ItemCode, obj.Month, obj.Month01Quantity, obj.Month02Quantity, obj.Month03Quantity, obj.Month04Quantity, obj.Month05Quantity, obj.Month06Quantity, obj.Month07Quantity, obj.Month08Quantity, obj.Month09Quantity, obj.Month10Quantity, obj.Month11Quantity, obj.Month12Quantity, obj.Narration, obj.Id, obj.SellingPrice, obj.UnitCostPrice, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
