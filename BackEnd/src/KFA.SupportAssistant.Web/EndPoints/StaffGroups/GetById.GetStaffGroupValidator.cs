using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetStaffGroupValidator : Validator<GetStaffGroupByIdRequest>
{
  public GetStaffGroupValidator()
  {
    RuleFor(x => x.GroupNumber)
      .NotEmpty()
      .WithMessage("The group number to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
