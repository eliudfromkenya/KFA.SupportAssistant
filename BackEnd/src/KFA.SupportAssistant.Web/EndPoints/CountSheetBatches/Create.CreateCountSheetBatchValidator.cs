using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateCountSheetBatchValidator : Validator<CreateCountSheetBatchRequest>
{
  public CreateCountSheetBatchValidator()
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
  }
}
