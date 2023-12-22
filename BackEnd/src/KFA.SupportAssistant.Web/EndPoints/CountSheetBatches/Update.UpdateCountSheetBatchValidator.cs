using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateCountSheetBatchValidator : Validator<UpdateCountSheetBatchRequest>
{
  public UpdateCountSheetBatchValidator()
  {
    RuleFor(x => x.BatchKey)
    .NotEmpty()
    .WithMessage("Batch Key is required.");

    RuleFor(x => x.BatchNumber)
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.ClassOfCard)
         .MinimumLength(1)
         .MaximumLength(8);

    RuleFor(x => x.CostCentreCode)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.Month)
         .MinimumLength(2)
         .MaximumLength(10);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.BatchKey)
      .Must((args, id) => checkIds(args.BatchKey, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
