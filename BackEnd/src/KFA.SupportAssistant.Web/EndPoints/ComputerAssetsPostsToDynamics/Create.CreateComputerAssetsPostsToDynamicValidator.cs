using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetsPostsToDynamicValidator : Validator<CreateComputerAssetsPostsToDynamicRequest>
{
  public CreateComputerAssetsPostsToDynamicValidator()
  {
    RuleFor(x => x.Description)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.PostID)
         .NotEmpty()
         .WithMessage("Post ID is required.");
  }
}
