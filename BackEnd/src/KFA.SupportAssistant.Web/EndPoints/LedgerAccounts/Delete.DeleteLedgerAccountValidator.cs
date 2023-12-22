using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteLedgerAccountValidator : Validator<DeleteLedgerAccountRequest>
{
  public DeleteLedgerAccountValidator()
  {
    RuleFor(x => x.LedgerAccountId)
      .NotEmpty()
      .WithMessage("The ledger account id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
