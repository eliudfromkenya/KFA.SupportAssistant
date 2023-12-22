using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateEmployeeDetailValidator : Validator<UpdateEmployeeDetailRequest>
{
  public UpdateEmployeeDetailValidator()
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

    RuleFor(x => x.EmployeeID)
         .NotEmpty()
         .WithMessage("Employee ID is required.");

    RuleFor(x => x.FullName)
         .NotEmpty()
         .WithMessage("Full Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Gender)
         .MinimumLength(1)
         .MaximumLength(8);

    RuleFor(x => x.GroupNumber)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.IdNumber)
         .MinimumLength(2)
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.EmployeeID)
      .Must((args, id) => checkIds(args.EmployeeID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
