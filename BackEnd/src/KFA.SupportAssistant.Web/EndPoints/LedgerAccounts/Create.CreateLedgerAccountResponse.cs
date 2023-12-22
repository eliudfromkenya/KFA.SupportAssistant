namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

public readonly struct CreateLedgerAccountResponse(string? costCentreCode, string? description, string? groupName, bool? increaseWithDebit, string? ledgerAccountCode, string? ledgerAccountId, string? mainGroup, string? narration, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Description { get; } = description;
  public string? GroupName { get; } = groupName;
  public bool? IncreaseWithDebit { get; } = increaseWithDebit;
  public string? LedgerAccountCode { get; } = ledgerAccountCode;
  public string? LedgerAccountId { get; } = ledgerAccountId;
  public string? MainGroup { get; } = mainGroup;
  public string? Narration { get; } = narration;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
