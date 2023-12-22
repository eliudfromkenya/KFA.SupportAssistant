using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchStaffGroupValidator : Validator<PatchStaffGroupRequest>
{
  public PatchStaffGroupValidator()
  {
    RuleFor(x => x.GroupNumber)
     .NotEmpty()
     .WithMessage("The group number of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
