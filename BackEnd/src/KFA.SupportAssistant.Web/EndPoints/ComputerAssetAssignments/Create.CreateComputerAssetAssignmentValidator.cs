using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateComputerAssetAssignmentValidator : Validator<CreateComputerAssetAssignmentRequest>
{
  public CreateComputerAssetAssignmentValidator()
  {
    RuleFor(x => x.AssignedUser)
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.AssignmentType)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.AssignmentID)
         .NotEmpty()
         .WithMessage("AssignmentID is required.");

    RuleFor(x => x.CostCentreCode)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.PayrollNumber)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
