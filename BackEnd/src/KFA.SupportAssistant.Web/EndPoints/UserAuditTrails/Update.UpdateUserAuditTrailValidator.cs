using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateUserAuditTrailValidator : Validator<UpdateUserAuditTrailRequest>
{
  public UpdateUserAuditTrailValidator()
  {
    RuleFor(x => x.ActivityDate)
    .NotEmpty()
    .WithMessage("Activity Date is required.");

    RuleFor(x => x.ActivityEnumNumber)
         .NotEmpty()
         .WithMessage("Activity Enum Number is required.");

    RuleFor(x => x.AuditId)
         .NotEmpty()
         .WithMessage("Audit Id is required.");

    RuleFor(x => x.Category)
         .NotEmpty()
         .WithMessage("Category is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.CommandId)
         .NotEmpty()
         .WithMessage("Command Id is required.");

    RuleFor(x => x.Data)
         .NotEmpty()
         .WithMessage("Data is required.");

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.LoginId)
         .NotEmpty()
         .WithMessage("Login Id is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.OldValues)
         .NotEmpty()
         .WithMessage("Old Values is required.");

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.AuditId)
      .Must((args, id) => checkIds(args.AuditId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
