using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateLetPropertiesAccountValidator : Validator<UpdateLetPropertiesAccountRequest>
{
  public UpdateLetPropertiesAccountValidator()
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

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.LetPropertyAccountId)
      .Must((args, id) => checkIds(args.LetPropertyAccountId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
