using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchUserAuditTrailValidator : Validator<PatchUserAuditTrailRequest>
{
  public PatchUserAuditTrailValidator()
  {
    RuleFor(x => x.AuditId)
     .NotEmpty()
     .WithMessage("The audit id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
