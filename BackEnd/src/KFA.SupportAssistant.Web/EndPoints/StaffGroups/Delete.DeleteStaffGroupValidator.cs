using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteStaffGroupValidator : Validator<DeleteStaffGroupRequest>
{
  public DeleteStaffGroupValidator()
  {
    RuleFor(x => x.GroupNumber)
      .NotEmpty()
      .WithMessage("The group number to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
