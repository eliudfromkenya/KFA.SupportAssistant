using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetGroupValidator : Validator<CreateComputerAssetGroupRequest>
{
  public CreateComputerAssetGroupValidator()
  {
    RuleFor(x => x.Description)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.GroupID)
         .NotEmpty()
         .WithMessage("Group ID is required.");

    RuleFor(x => x.GroupName)
         .NotEmpty()
         .WithMessage("Group Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.LastAssignedValue)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Prefix)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Suffix)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
