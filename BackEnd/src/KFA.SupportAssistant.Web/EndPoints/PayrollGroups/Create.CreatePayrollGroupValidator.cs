using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreatePayrollGroupValidator : Validator<CreatePayrollGroupRequest>
{
  public CreatePayrollGroupValidator()
  {
    RuleFor(x => x.GroupID)
    .NotEmpty()
    .WithMessage("Group ID is required.");

    RuleFor(x => x.GroupName)
         .NotEmpty()
         .WithMessage("Group Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);
  }
}
