using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetMaintainceValidator : Validator<DeleteComputerAssetMaintainceRequest>
{
  public DeleteComputerAssetMaintainceValidator()
  {
    RuleFor(x => x.MaintainceID)
      .NotEmpty()
      .WithMessage("The maintaince id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
