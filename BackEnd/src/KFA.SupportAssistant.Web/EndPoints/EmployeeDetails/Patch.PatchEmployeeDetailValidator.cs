using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchEmployeeDetailValidator : Validator<PatchEmployeeDetailRequest>
{
  public PatchEmployeeDetailValidator()
  {
    RuleFor(x => x.EmployeeID)
     .NotEmpty()
     .WithMessage("The employee id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
