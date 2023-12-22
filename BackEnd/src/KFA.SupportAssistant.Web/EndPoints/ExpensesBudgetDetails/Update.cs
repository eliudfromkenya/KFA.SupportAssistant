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

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// Update an existing expenses budget detail.
/// </summary>
/// <remarks>
/// Update an existing expenses budget detail by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateExpensesBudgetDetailRequest, UpdateExpensesBudgetDetailResponse>
{
  private const string EndPointId = "ENP-1D7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateExpensesBudgetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Expenses Budget Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Expenses Budget Detail";
      s.Description = "This endpoint is used to update  expenses budget detail, making a full replacement of expenses budget detail with a specifed valuse. A valid expenses budget detail is required.";
      s.ExampleRequest = new UpdateExpensesBudgetDetailRequest { BasisOfCalculation = "Basis Of Calculation", BatchKey = "Batch Key", ExpenseBudgetDetailId = "1000", LedgerAccountCode = "Ledger Account Code", Month01 = 0, Month02 = 0, Month03 = 0, Month04 = 0, Month05 = 0, Month06 = 0, Month07 = 0, Month08 = 0, Month09 = 0, Month10 = 0, Month11 = 0, Month12 = 0, Narration = "Narration" };
      s.ResponseExamples[200] = new UpdateExpensesBudgetDetailResponse(new ExpensesBudgetDetailRecord("Basis Of Calculation", "Batch Key", "1000", "Ledger Account Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateExpensesBudgetDetailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ExpenseBudgetDetailId))
    {
      AddError(request => request.ExpenseBudgetDetailId, "The expense budget detail id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ExpensesBudgetDetailDTO, ExpensesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.ExpenseBudgetDetailId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the expenses budget detail to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ExpensesBudgetDetailDTO, ExpensesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.ExpenseBudgetDetailId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateExpensesBudgetDetailResponse(new ExpensesBudgetDetailRecord(obj.BasisOfCalculation, obj.BatchKey, obj.Id, obj.LedgerAccountCode, obj.Month01, obj.Month02, obj.Month03, obj.Month04, obj.Month05, obj.Month06, obj.Month07, obj.Month08, obj.Month09, obj.Month10, obj.Month11, obj.Month12, obj.Narration, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
