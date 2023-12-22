using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetCountSheetBatchValidator : Validator<GetCountSheetBatchByIdRequest>
{
  public GetCountSheetBatchValidator()
  {
    RuleFor(x => x.BatchKey)
      .NotEmpty()
      .WithMessage("The batch key to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
