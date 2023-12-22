using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetPasswordSafeValidator : Validator<GetPasswordSafeByIdRequest>
{
  public GetPasswordSafeValidator()
  {
    RuleFor(x => x.PasswordId)
      .NotEmpty()
      .WithMessage("The password id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
