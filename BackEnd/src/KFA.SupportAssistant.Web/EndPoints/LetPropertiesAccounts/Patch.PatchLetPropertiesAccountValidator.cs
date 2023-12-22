using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchLetPropertiesAccountValidator : Validator<PatchLetPropertiesAccountRequest>
{
  public PatchLetPropertiesAccountValidator()
  {
    RuleFor(x => x.LetPropertyAccountId)
     .NotEmpty()
     .WithMessage("The let property account id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
