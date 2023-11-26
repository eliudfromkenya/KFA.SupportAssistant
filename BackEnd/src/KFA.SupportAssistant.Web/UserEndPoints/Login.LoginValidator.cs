using FluentValidation;
using KFA.SupportAssistant.Infrastructure.Data.Config;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class LoginValidator : Validator<LoginRequest>
{
  public LoginValidator()
  {
    RuleFor(x => x.Username)
      .NotEmpty()
      .WithMessage("Username is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.Password)
    .NotEmpty()
    .WithMessage("Password is required.")
    .MinimumLength(2)
    .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
  }
}
