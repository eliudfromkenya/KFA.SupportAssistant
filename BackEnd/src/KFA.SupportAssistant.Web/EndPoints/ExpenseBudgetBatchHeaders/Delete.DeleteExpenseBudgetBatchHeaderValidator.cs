using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteExpenseBudgetBatchHeaderValidator : Validator<DeleteExpenseBudgetBatchHeaderRequest>
{
  public DeleteExpenseBudgetBatchHeaderValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
