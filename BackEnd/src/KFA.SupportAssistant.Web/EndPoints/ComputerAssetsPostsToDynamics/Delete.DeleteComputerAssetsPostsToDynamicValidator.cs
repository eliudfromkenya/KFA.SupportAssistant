using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetsPostsToDynamicValidator : Validator<DeleteComputerAssetsPostsToDynamicRequest>
{
  public DeleteComputerAssetsPostsToDynamicValidator()
  {
    RuleFor(x => x.PostID)
      .NotEmpty()
      .WithMessage("The post id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
