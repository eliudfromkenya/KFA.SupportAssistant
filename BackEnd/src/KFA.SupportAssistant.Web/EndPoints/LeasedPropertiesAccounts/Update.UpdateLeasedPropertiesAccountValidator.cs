using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateLeasedPropertiesAccountValidator : Validator<UpdateLeasedPropertiesAccountRequest>
{
  public UpdateLeasedPropertiesAccountValidator()
  {
    RuleFor(x => x.CostCentreCode)
    .MinimumLength(2)
    .MaximumLength(25);

    RuleFor(x => x.LandlordAddress)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.LeasedPropertyAccountId)
         .NotEmpty()
         .WithMessage("Leased Property Account Id is required.");

    RuleFor(x => x.LedgerAccountCode)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.LeasedPropertyAccountId)
      .Must((args, id) => checkIds(args.LeasedPropertyAccountId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
