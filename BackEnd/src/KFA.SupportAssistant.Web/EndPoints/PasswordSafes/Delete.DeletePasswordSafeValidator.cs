using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeletePasswordSafeValidator : Validator<DeletePasswordSafeRequest>
{
  public DeletePasswordSafeValidator()
  {
    RuleFor(x => x.PasswordId)
      .NotEmpty()
      .WithMessage("The password id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
