using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeletePayrollGroupValidator : Validator<DeletePayrollGroupRequest>
{
  public DeletePayrollGroupValidator()
  {
    RuleFor(x => x.GroupID)
      .NotEmpty()
      .WithMessage("The group id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
