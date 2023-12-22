using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdatePasswordSafeValidator : Validator<UpdatePasswordSafeRequest>
{
  public UpdatePasswordSafeValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.PasswordId)
      .Must((args, id) => checkIds(args.PasswordId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
