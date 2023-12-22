using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchLeasedPropertiesAccountValidator : Validator<PatchLeasedPropertiesAccountRequest>
{
  public PatchLeasedPropertiesAccountValidator()
  {
    RuleFor(x => x.LeasedPropertyAccountId)
     .NotEmpty()
     .WithMessage("The leased property account id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
