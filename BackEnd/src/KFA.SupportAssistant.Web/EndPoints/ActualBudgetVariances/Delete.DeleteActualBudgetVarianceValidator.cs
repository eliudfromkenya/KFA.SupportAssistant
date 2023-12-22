using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteActualBudgetVarianceValidator : Validator<DeleteActualBudgetVarianceRequest>
{
  public DeleteActualBudgetVarianceValidator()
  {
    RuleFor(x => x.ActualBudgetID)
      .NotEmpty()
      .WithMessage("The actual budget id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
