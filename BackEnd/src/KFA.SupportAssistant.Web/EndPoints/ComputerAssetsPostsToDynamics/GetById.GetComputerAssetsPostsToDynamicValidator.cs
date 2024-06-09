using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetsPostsToDynamicValidator : Validator<GetComputerAssetsPostsToDynamicByIdRequest>
{
  public GetComputerAssetsPostsToDynamicValidator()
  {
    RuleFor(x => x.PostID)
      .NotEmpty()
      .WithMessage("The post id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
