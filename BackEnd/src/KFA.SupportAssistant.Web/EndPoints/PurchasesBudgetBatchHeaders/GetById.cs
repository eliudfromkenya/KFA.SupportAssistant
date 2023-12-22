using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

/// <summary>
/// Get a purchases budget batch header by batch key.
/// </summary>
/// <remarks>
/// Takes batch key and returns a matching purchases budget batch header record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetPurchasesBudgetBatchHeaderByIdRequest, PurchasesBudgetBatchHeaderRecord>
{
  private const string EndPointId = "ENP-1P4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetPurchasesBudgetBatchHeaderByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Purchases Budget Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets purchases budget batch header by specified batch key";
      s.Description = "This endpoint is used to retrieve purchases budget batch header with the provided batch key";
      s.ExampleRequest = new GetPurchasesBudgetBatchHeaderByIdRequest { BatchKey = "batch key to retrieve" };
      s.ResponseExamples[200] = new PurchasesBudgetBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetPurchasesBudgetBatchHeaderByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The batch key of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<PurchasesBudgetBatchHeaderDTO, PurchasesBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "");
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
      Response = new PurchasesBudgetBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.TotalQuantity, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
