using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetsPostsToDynamicValidator : Validator<PatchComputerAssetsPostsToDynamicRequest>
{
  public PatchComputerAssetsPostsToDynamicValidator()
  {
    RuleFor(x => x.PostID)
     .NotEmpty()
     .WithMessage("The post id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

