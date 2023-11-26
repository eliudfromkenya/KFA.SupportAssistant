using FluentValidation;
using KFA.SupportAssistant.Infrastructure.Data.Config;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class ChangePasswordValidator : Validator<ChangePasswordRequest>
{
  public ChangePasswordValidator()
  {
    RuleFor(x => x.Username)
      .NotEmpty()
      .WithMessage("Username is required.")
      .MinimumLength(2)
      .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.CurrentPassword)
    .NotEmpty()
    .WithMessage("Current password is required.")
    .MinimumLength(2)
    .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    RuleFor(x => x.NewPassword)
   .NotEmpty()
   .WithMessage("New password is required.")
   .MinimumLength(2)
   .MaximumLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);
  }
}
