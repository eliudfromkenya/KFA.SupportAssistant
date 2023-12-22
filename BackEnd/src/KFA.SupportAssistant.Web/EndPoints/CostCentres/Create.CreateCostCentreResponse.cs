namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

public readonly struct CreateCostCentreResponse(string? costCentreCode, string? description, string? narration, string? region, string? supplierCodePrefix, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Description { get; } = description;
  public string? Narration { get; } = narration;
  public string? Region { get; } = region;
  public string? SupplierCodePrefix { get; } = supplierCodePrefix;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
