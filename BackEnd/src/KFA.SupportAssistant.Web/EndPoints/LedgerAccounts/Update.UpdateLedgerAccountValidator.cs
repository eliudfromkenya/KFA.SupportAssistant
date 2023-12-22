using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateLedgerAccountValidator : Validator<UpdateLedgerAccountRequest>
{
  public UpdateLedgerAccountValidator()
  {
    RuleFor(x => x.CostCentreCode)
    .MinimumLength(2)
    .MaximumLength(25);

    RuleFor(x => x.Description)
         .NotEmpty()
         .WithMessage("Description is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.GroupName)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.LedgerAccountCode)
         .NotEmpty()
         .WithMessage("Ledger Account Code is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.LedgerAccountId)
         .NotEmpty()
         .WithMessage("Ledger Account Id is required.")
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.MainGroup)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.LedgerAccountId)
      .Must((args, id) => checkIds(args.LedgerAccountId, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
