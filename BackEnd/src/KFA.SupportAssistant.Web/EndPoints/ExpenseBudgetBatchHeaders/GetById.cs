using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// Get a expense budget batch header by batch key.
/// </summary>
/// <remarks>
/// Takes batch key and returns a matching expense budget batch header record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetExpenseBudgetBatchHeaderByIdRequest, ExpenseBudgetBatchHeaderRecord>
{
  private const string EndPointId = "ENP-1C4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetExpenseBudgetBatchHeaderByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Expense Budget Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets expense budget batch header by specified batch key";
      s.Description = "This endpoint is used to retrieve expense budget batch header with the provided batch key";
      s.ExampleRequest = new GetExpenseBudgetBatchHeaderByIdRequest { BatchKey = "batch key to retrieve" };
      s.ResponseExamples[200] = new ExpenseBudgetBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetExpenseBudgetBatchHeaderByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The batch key of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "");
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
      Response = new ExpenseBudgetBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
