using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteUserAuditTrailValidator : Validator<DeleteUserAuditTrailRequest>
{
  public DeleteUserAuditTrailValidator()
  {
    RuleFor(x => x.AuditId)
      .NotEmpty()
      .WithMessage("The audit id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
