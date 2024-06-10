using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetsMaintainceAccessoryValidator : Validator<PatchComputerAssetsMaintainceAccessoryRequest>
{
  public PatchComputerAssetsMaintainceAccessoryValidator()
  {
    RuleFor(x => x.AccessoryID)
     .NotEmpty()
     .WithMessage("The accessory id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
