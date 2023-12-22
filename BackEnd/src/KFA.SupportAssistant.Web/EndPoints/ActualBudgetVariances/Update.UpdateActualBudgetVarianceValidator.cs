using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateActualBudgetVarianceValidator : Validator<UpdateActualBudgetVarianceRequest>
{
  public UpdateActualBudgetVarianceValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.ActualBudgetID)
      .Must((args, id) => checkIds(args.ActualBudgetID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
