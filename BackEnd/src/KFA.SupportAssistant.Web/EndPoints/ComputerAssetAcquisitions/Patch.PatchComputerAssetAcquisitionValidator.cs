using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetAcquisitionValidator : Validator<PatchComputerAssetAcquisitionRequest>
{
  public PatchComputerAssetAcquisitionValidator()
  {
    RuleFor(x => x.AcquisitionID)
     .NotEmpty()
     .WithMessage("The acquisition id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

