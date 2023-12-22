namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

public class UpdateLedgerAccountResponse
{
  public UpdateLedgerAccountResponse(LedgerAccountRecord ledgerAccount)
  {
    LedgerAccount = ledgerAccount;
  }

  public LedgerAccountRecord LedgerAccount { get; set; }
}
