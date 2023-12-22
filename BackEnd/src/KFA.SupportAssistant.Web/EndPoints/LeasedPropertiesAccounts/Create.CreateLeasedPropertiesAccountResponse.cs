namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

public readonly struct CreateLeasedPropertiesAccountResponse(decimal? commencementRent, string? costCentreCode, decimal? currentRent, string? landlordAddress, global::System.DateTime? lastReviewDate, global::System.DateTime? leasedOn, string? leasedPropertyAccountId, string? ledgerAccountCode, string? narration, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public decimal? CommencementRent { get; } = commencementRent;
  public string? CostCentreCode { get; } = costCentreCode;
  public decimal? CurrentRent { get; } = currentRent;
  public string? LandlordAddress { get; } = landlordAddress;
  public global::System.DateTime? LastReviewDate { get; } = lastReviewDate;
  public global::System.DateTime? LeasedOn { get; } = leasedOn;
  public string? LeasedPropertyAccountId { get; } = leasedPropertyAccountId;
  public string? LedgerAccountCode { get; } = ledgerAccountCode;
  public string? Narration { get; } = narration;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
