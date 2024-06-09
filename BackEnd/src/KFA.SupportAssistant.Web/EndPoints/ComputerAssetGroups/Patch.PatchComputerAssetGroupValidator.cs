using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetGroupValidator : Validator<PatchComputerAssetGroupRequest>
{
  public PatchComputerAssetGroupValidator()
  {
    RuleFor(x => x.GroupID)
     .NotEmpty()
     .WithMessage("The group id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

