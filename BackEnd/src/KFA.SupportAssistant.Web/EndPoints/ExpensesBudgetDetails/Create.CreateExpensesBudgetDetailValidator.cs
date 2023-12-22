using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateExpensesBudgetDetailValidator : Validator<CreateExpensesBudgetDetailRequest>
{
  public CreateExpensesBudgetDetailValidator()
  {
    RuleFor(x => x.BasisOfCalculation)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.ExpenseBudgetDetailId)
         .NotEmpty()
         .WithMessage("Expense Budget Detail Id is required.");

    RuleFor(x => x.LedgerAccountCode)
         .MinimumLength(2)
         .MaximumLength(15);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);
  }
}
