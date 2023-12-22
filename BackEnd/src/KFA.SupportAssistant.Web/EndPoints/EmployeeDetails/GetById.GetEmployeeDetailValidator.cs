using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetEmployeeDetailValidator : Validator<GetEmployeeDetailByIdRequest>
{
  public GetEmployeeDetailValidator()
  {
    RuleFor(x => x.EmployeeID)
      .NotEmpty()
      .WithMessage("The employee id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
