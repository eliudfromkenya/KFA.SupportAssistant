using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetComputerAssetAssignmentValidator : Validator<GetComputerAssetAssignmentByIdRequest>
{
  public GetComputerAssetAssignmentValidator()
  {
    RuleFor(x => x.AssignmentID)
      .NotEmpty()
      .WithMessage("The assignmentid to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
