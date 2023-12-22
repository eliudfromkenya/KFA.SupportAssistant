using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateStaffGroupValidator : Validator<CreateStaffGroupRequest>
{
  public CreateStaffGroupValidator()
  {
    RuleFor(x => x.Description)
    .NotEmpty()
    .WithMessage("Description is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.GroupNumber)
         .NotEmpty()
         .WithMessage("Group Number is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);
  }
}
