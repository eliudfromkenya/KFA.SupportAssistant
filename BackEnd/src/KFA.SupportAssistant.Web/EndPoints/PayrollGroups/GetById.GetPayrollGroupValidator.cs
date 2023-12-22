using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetPayrollGroupValidator : Validator<GetPayrollGroupByIdRequest>
{
  public GetPayrollGroupValidator()
  {
    RuleFor(x => x.GroupID)
      .NotEmpty()
      .WithMessage("The group id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
