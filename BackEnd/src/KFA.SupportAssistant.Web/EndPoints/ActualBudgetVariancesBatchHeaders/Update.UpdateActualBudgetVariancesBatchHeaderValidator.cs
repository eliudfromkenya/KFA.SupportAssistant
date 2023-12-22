using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateActualBudgetVariancesBatchHeaderValidator : Validator<UpdateActualBudgetVariancesBatchHeaderRequest>
{
  public UpdateActualBudgetVariancesBatchHeaderValidator()
  {
    RuleFor(x => x.ApprovedBy)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.BatchKey)
         .NotEmpty()
         .WithMessage("Batch Key is required.");

    RuleFor(x => x.BatchNumber)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.CostCentreCode)
         .NotEmpty()
         .WithMessage("Cost Centre Code is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.Month)
         .NotEmpty()
         .WithMessage("Month is required.")
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PreparedBy)
         .MinimumLength(2)
         .MaximumLength(255);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.BatchKey)
      .Must((args, id) => checkIds(args.BatchKey, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
