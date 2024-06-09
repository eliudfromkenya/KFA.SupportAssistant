namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

public readonly struct CreateComputerAssetValuationResponse(string? assetDetailID, string? description, string? revaluationID, string? status, global::System.DateTime? valuationDate, decimal? value, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDetailID { get; } = assetDetailID;
  public string? Description { get; } = description;
  public string? RevaluationID { get; } = revaluationID;
  public string? Status { get; } = status;
  public global::System.DateTime? ValuationDate { get; } = valuationDate;
  public decimal? Value { get; } = value;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
