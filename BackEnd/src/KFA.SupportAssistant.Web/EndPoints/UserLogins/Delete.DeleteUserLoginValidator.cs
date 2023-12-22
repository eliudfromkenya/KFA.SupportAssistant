using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteUserLoginValidator : Validator<DeleteUserLoginRequest>
{
  public DeleteUserLoginValidator()
  {
    RuleFor(x => x.LoginId)
      .NotEmpty()
      .WithMessage("The login id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
