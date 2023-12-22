using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchExpensesBudgetDetailValidator : Validator<PatchExpensesBudgetDetailRequest>
{
  public PatchExpensesBudgetDetailValidator()
  {
    RuleFor(x => x.ExpenseBudgetDetailId)
     .NotEmpty()
     .WithMessage("The expense budget detail id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
