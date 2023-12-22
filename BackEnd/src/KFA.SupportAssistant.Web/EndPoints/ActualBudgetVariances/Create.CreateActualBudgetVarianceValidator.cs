using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateActualBudgetVarianceValidator : Validator<CreateActualBudgetVarianceRequest>
{
  public CreateActualBudgetVarianceValidator()
  {
    RuleFor(x => x.ActualBudgetID)
    .NotEmpty()
    .WithMessage("Actual Budget ID is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.ActualValue)
         .NotEmpty()
         .WithMessage("Actual Value is required.");

    RuleFor(x => x.Comment)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Field1)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Field2)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Field3)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);
  }
}
