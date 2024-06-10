using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetGroupValidator : Validator<DeleteComputerAssetGroupRequest>
{
  public DeleteComputerAssetGroupValidator()
  {
    RuleFor(x => x.GroupID)
      .NotEmpty()
      .WithMessage("The group id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
