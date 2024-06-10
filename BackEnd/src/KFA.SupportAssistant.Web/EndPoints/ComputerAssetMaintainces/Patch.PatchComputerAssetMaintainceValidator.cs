using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetMaintainceValidator : Validator<PatchComputerAssetMaintainceRequest>
{
  public PatchComputerAssetMaintainceValidator()
  {
    RuleFor(x => x.MaintainceID)
     .NotEmpty()
     .WithMessage("The maintaince id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
