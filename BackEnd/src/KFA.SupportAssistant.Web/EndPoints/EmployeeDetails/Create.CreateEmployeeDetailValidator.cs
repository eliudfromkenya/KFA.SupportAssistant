using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateEmployeeDetailValidator : Validator<CreateEmployeeDetailRequest>
{
  public CreateEmployeeDetailValidator()
  {
    RuleFor(x => x.Classification)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.CostCentreCode)
         .MinimumLength(1)
         .MaximumLength(6);

    RuleFor(x => x.Email)
         .MinimumLength(2)
         .MaximumLength(255);

    //RuleFor(x => x.EmployeeID)
    //     .NotEmpty()
    //     .WithMessage("Employee ID is required.");

    RuleFor(x => x.FullName)
         .NotEmpty()
         .WithMessage("Full Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Gender)
         .MinimumLength(1)
         .MaximumLength(8);

    RuleFor(x => x.GroupNumber)
         .MinimumLength(1)
         .MaximumLength(25);

    RuleFor(x => x.IdNumber)
         .MinimumLength(5)
         .MaximumLength(15);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PayrollNumber)
         .MinimumLength(1)
         .MaximumLength(8);

    RuleFor(x => x.PhoneNumber)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.Remarks)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Status)
         .MinimumLength(2)
         .MaximumLength(10);
  }
}
