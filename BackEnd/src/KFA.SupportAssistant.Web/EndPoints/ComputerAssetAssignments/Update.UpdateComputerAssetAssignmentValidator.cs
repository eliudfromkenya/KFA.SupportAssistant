using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateComputerAssetAssignmentValidator : Validator<UpdateComputerAssetAssignmentRequest>
{
  public UpdateComputerAssetAssignmentValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.AssignmentID)
      .Must((args, id) => checkIds(args.AssignmentID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
