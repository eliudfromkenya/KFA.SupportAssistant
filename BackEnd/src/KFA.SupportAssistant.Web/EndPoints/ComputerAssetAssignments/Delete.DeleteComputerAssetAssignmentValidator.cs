using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteComputerAssetAssignmentValidator : Validator<DeleteComputerAssetAssignmentRequest>
{
  public DeleteComputerAssetAssignmentValidator()
  {
    RuleFor(x => x.AssignmentID)
      .NotEmpty()
      .WithMessage("The assignmentid to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}

