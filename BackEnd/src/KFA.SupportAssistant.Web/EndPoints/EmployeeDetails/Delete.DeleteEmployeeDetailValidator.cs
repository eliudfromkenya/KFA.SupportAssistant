using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteEmployeeDetailValidator : Validator<DeleteEmployeeDetailRequest>
{
  public DeleteEmployeeDetailValidator()
  {
    RuleFor(x => x.EmployeeID)
      .NotEmpty()
      .WithMessage("The employee id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
