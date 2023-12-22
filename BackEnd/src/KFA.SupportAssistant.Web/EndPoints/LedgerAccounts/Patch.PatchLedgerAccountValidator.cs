using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchLedgerAccountValidator : Validator<PatchLedgerAccountRequest>
{
  public PatchLedgerAccountValidator()
  {
    RuleFor(x => x.LedgerAccountId)
     .NotEmpty()
     .WithMessage("The ledger account id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
