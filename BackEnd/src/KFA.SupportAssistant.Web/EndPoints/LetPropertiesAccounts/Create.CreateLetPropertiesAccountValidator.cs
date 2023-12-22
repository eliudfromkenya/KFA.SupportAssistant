using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateLetPropertiesAccountValidator : Validator<CreateLetPropertiesAccountRequest>
{
  public CreateLetPropertiesAccountValidator()
  {
    RuleFor(x => x.CostCentreCode)
    .MinimumLength(2)
    .MaximumLength(25);

    RuleFor(x => x.LedgerAccountCode)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.LetPropertyAccountId)
         .NotEmpty()
         .WithMessage("Let Property Account Id is required.");

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.TenantAddress)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
