using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteExpensesBudgetDetailValidator : Validator<DeleteExpensesBudgetDetailRequest>
{
  public DeleteExpensesBudgetDetailValidator()
  {
    RuleFor(x => x.ExpenseBudgetDetailId)
      .NotEmpty()
      .WithMessage("The expense budget detail id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
