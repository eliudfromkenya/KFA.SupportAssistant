using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetLedgerAccountValidator : Validator<GetLedgerAccountByIdRequest>
{
  public GetLedgerAccountValidator()
  {
    RuleFor(x => x.LedgerAccountId)
      .NotEmpty()
      .WithMessage("The ledger account id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
