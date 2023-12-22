using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// Get a expenses budget detail by expense budget detail id.
/// </summary>
/// <remarks>
/// Takes expense budget detail id and returns a matching expenses budget detail record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetExpensesBudgetDetailByIdRequest, ExpensesBudgetDetailRecord>
{
  private const string EndPointId = "ENP-1D4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetExpensesBudgetDetailByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Expenses Budget Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets expenses budget detail by specified expense budget detail id";
      s.Description = "This endpoint is used to retrieve expenses budget detail with the provided expense budget detail id";
      s.ExampleRequest = new GetExpensesBudgetDetailByIdRequest { ExpenseBudgetDetailId = "expense budget detail id to retrieve" };
      s.ResponseExamples[200] = new ExpensesBudgetDetailRecord("Basis Of Calculation", "Batch Key", "1000", "Ledger Account Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetExpensesBudgetDetailByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ExpenseBudgetDetailId))
    {
      AddError(request => request.ExpenseBudgetDetailId, "The expense budget detail id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ExpensesBudgetDetailDTO, ExpensesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.ExpenseBudgetDetailId ?? "");
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
      Response = new ExpensesBudgetDetailRecord(obj.BasisOfCalculation, obj.BatchKey, obj.Id, obj.LedgerAccountCode, obj.Month01, obj.Month02, obj.Month03, obj.Month04, obj.Month05, obj.Month06, obj.Month07, obj.Month08, obj.Month09, obj.Month10, obj.Month11, obj.Month12, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
