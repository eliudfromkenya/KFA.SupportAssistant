using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateExpensesBudgetDetailValidator : Validator<UpdateExpensesBudgetDetailRequest>
{
  public UpdateExpensesBudgetDetailValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.ExpenseBudgetDetailId)
      .Must((args, id) => checkIds(args.ExpenseBudgetDetailId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
