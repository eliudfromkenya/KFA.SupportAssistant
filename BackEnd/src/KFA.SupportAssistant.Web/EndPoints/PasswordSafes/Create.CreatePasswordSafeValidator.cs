using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreatePasswordSafeValidator : Validator<CreatePasswordSafeRequest>
{
  public CreatePasswordSafeValidator()
  {
    RuleFor(x => x.Details)
    .NotEmpty()
    .WithMessage("Details is required.")
    .MinimumLength(2)
    .MaximumLength(255);

    RuleFor(x => x.Name)
         .NotEmpty()
         .WithMessage("Name is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Password)
         .NotEmpty()
         .WithMessage("Password is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.PasswordId)
         .NotEmpty()
         .WithMessage("Password Id is required.");

    RuleFor(x => x.Reminder)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
