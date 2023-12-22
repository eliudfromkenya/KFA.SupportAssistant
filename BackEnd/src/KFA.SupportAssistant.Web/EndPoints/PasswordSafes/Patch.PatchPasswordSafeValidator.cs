using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchPasswordSafeValidator : Validator<PatchPasswordSafeRequest>
{
  public PatchPasswordSafeValidator()
  {
    RuleFor(x => x.PasswordId)
     .NotEmpty()
     .WithMessage("The password id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
