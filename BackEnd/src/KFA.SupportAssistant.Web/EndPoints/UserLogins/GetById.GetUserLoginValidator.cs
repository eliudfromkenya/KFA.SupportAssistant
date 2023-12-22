using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetUserLoginValidator : Validator<GetUserLoginByIdRequest>
{
  public GetUserLoginValidator()
  {
    RuleFor(x => x.LoginId)
      .NotEmpty()
      .WithMessage("The login id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
