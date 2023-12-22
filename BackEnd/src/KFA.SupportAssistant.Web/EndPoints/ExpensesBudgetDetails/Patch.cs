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

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchExpensesBudgetDetailRequest, ExpensesBudgetDetailRecord>
{
  private const string EndPointId = "ENP-1D6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchExpensesBudgetDetailRequest.Route));
    //RequestBinder(new PatchBinder<ExpensesBudgetDetailDTO, ExpensesBudgetDetail, PatchExpensesBudgetDetailRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Expenses Budget Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a expenses budget detail";
      s.Description = "Used to update part of an existing expenses budget detail. A valid existing expenses budget detail is required.";
      s.ResponseExamples[200] = new ExpensesBudgetDetailRecord("Basis Of Calculation", "Batch Key", "1000", "Ledger Account Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchExpensesBudgetDetailRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ExpenseBudgetDetailId))
    {
      AddError(request => request.ExpenseBudgetDetailId, "The expenses budget detail of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ExpensesBudgetDetailDTO patchFunc(ExpensesBudgetDetailDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ExpensesBudgetDetailDTO, ExpensesBudgetDetail>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ExpensesBudgetDetailDTO, ExpensesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.ExpenseBudgetDetailId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the expenses budget detail to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ExpensesBudgetDetailRecord(obj.BasisOfCalculation, obj.BatchKey, obj.Id, obj.LedgerAccountCode, obj.Month01, obj.Month02, obj.Month03, obj.Month04, obj.Month05, obj.Month06, obj.Month07, obj.Month08, obj.Month09, obj.Month10, obj.Month11, obj.Month12, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
