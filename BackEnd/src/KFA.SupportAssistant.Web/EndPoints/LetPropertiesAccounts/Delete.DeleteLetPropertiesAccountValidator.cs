using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteLetPropertiesAccountValidator : Validator<DeleteLetPropertiesAccountRequest>
{
  public DeleteLetPropertiesAccountValidator()
  {
    RuleFor(x => x.LetPropertyAccountId)
      .NotEmpty()
      .WithMessage("The let property account id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
