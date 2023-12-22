using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateItemGroupValidator : Validator<CreateItemGroupRequest>
{
  public CreateItemGroupValidator()
  {
    RuleFor(x => x.GroupId)
    .NotEmpty()
    .WithMessage("Group Id is required.");

    RuleFor(x => x.Name)
         .NotEmpty()
         .WithMessage("Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ParentGroupId)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
