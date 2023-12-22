using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdatePayrollGroupValidator : Validator<UpdatePayrollGroupRequest>
{
  public UpdatePayrollGroupValidator()
  {
    RuleFor(x => x.GroupID)
    .NotEmpty()
    .WithMessage("Group ID is required.");

    RuleFor(x => x.GroupName)
         .NotEmpty()
         .WithMessage("Group Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.GroupID)
      .Must((args, id) => checkIds(args.GroupID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
