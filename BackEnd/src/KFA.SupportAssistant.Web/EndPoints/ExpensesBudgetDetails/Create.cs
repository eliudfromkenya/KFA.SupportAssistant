using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// Create a new ExpensesBudgetDetail
/// </summary>
/// <remarks>
/// Creates a new expenses budget detail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateExpensesBudgetDetailRequest, CreateExpensesBudgetDetailResponse>
{
  private const string EndPointId = "ENP-1D1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateExpensesBudgetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Expenses Budget Detail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new expenses budget detail";
      s.Description = "This endpoint is used to create a new  expenses budget detail. Here details of expenses budget detail to be created is provided";
      s.ExampleRequest = new CreateExpensesBudgetDetailRequest { BasisOfCalculation = "Basis Of Calculation", BatchKey = "Batch Key", ExpenseBudgetDetailId = "1000", LedgerAccountCode = "Ledger Account Code", Month01 = 0, Month02 = 0, Month03 = 0, Month04 = 0, Month05 = 0, Month06 = 0, Month07 = 0, Month08 = 0, Month09 = 0, Month10 = 0, Month11 = 0, Month12 = 0, Narration = "Narration" };
      s.ResponseExamples[200] = new CreateExpensesBudgetDetailResponse("Basis Of Calculation", "Batch Key", "1000", "Ledger Account Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateExpensesBudgetDetailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ExpensesBudgetDetailDTO>();
    requestDTO.Id = request.ExpenseBudgetDetailId;

    var result = await mediator.Send(new CreateModelCommand<ExpensesBudgetDetailDTO, ExpensesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ExpensesBudgetDetailDTO obj)
      {
        Response = new CreateExpensesBudgetDetailResponse(obj.BasisOfCalculation, obj.BatchKey, obj.Id, obj.LedgerAccountCode, obj.Month01, obj.Month02, obj.Month03, obj.Month04, obj.Month05, obj.Month06, obj.Month07, obj.Month08, obj.Month09, obj.Month10, obj.Month11, obj.Month12, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
