using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetDetailValidator : Validator<PatchComputerAssetDetailRequest>
{
  public PatchComputerAssetDetailValidator()
  {
    RuleFor(x => x.AssetID)
     .NotEmpty()
     .WithMessage("The asset id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

