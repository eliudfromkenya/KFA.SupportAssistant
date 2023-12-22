namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

public readonly struct CreateLetPropertiesAccountResponse(decimal? commencementRent, string? costCentreCode, decimal? currentRent, global::System.DateTime? lastReviewDate, string? ledgerAccountCode, global::System.DateTime? letOn, string? letPropertyAccountId, string? narration, string? tenantAddress, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public decimal? CommencementRent { get; } = commencementRent;
  public string? CostCentreCode { get; } = costCentreCode;
  public decimal? CurrentRent { get; } = currentRent;
  public global::System.DateTime? LastReviewDate { get; } = lastReviewDate;
  public string? LedgerAccountCode { get; } = ledgerAccountCode;
  public global::System.DateTime? LetOn { get; } = letOn;
  public string? LetPropertyAccountId { get; } = letPropertyAccountId;
  public string? Narration { get; } = narration;
  public string? TenantAddress { get; } = tenantAddress;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
