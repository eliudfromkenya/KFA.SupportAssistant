using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchUserLoginValidator : Validator<PatchUserLoginRequest>
{
  public PatchUserLoginValidator()
  {
    RuleFor(x => x.LoginId)
     .NotEmpty()
     .WithMessage("The login id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
