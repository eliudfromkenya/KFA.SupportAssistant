using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetUserAuditTrailValidator : Validator<GetUserAuditTrailByIdRequest>
{
  public GetUserAuditTrailValidator()
  {
    RuleFor(x => x.AuditId)
      .NotEmpty()
      .WithMessage("The audit id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
