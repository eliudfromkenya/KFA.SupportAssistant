using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetValuationValidator : Validator<PatchComputerAssetValuationRequest>
{
  public PatchComputerAssetValuationValidator()
  {
    RuleFor(x => x.RevaluationID)
     .NotEmpty()
     .WithMessage("The revaluation id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
