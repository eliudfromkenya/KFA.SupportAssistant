namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public readonly struct CreateCostCentreResponse(string? costCentreCode, bool? isActive, string? description, string? narration, string? region, string? supplierCodePrefix, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Description { get; } = description;
  public string? Narration { get; } = narration;
  public string? Region { get; } = region;
  public string? SupplierCodePrefix { get; } = supplierCodePrefix;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
  public bool? IsActive { get; } = isActive;
}
