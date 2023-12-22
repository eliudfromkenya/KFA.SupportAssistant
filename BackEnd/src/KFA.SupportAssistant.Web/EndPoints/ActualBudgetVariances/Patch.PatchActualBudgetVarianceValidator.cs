using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchActualBudgetVarianceValidator : Validator<PatchActualBudgetVarianceRequest>
{
  public PatchActualBudgetVarianceValidator()
  {
    RuleFor(x => x.ActualBudgetID)
     .NotEmpty()
     .WithMessage("The actual budget id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
