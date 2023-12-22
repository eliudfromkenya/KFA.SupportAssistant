using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteCountSheetBatchValidator : Validator<DeleteCountSheetBatchRequest>
{
  public DeleteCountSheetBatchValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
