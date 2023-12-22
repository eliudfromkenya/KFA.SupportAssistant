using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateVerificationRightValidator : Validator<CreateVerificationRightRequest>
{
  public CreateVerificationRightValidator()
  {
    RuleFor(x => x.UserRoleId)
    .NotEmpty()
    .WithMessage("User Role Id is required.");

    RuleFor(x => x.VerificationRightId)
         .NotEmpty()
         .WithMessage("Verification Right Id is required.");
  }
}
