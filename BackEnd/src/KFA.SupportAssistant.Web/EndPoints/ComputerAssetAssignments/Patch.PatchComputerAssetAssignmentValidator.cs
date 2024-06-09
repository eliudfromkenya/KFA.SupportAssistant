using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchComputerAssetAssignmentValidator : Validator<PatchComputerAssetAssignmentRequest>
{
  public PatchComputerAssetAssignmentValidator()
  {
    RuleFor(x => x.AssignmentID)
     .NotEmpty()
     .WithMessage("The assignmentid of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}

