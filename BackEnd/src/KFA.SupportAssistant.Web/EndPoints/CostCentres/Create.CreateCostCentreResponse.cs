namespace KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;

public readonly struct CreateCostCentreResponse
{
  public CreateCostCentreResponse(string? costCentreCode, bool? isActive, string? description, string? narration, string? region, string? supplierCodePrefix, DateTime? dateInserted___, DateTime? dateUpdated___)
  {
    IsActive = isActive;
    CostCentreCode = costCentreCode;
    Description = description;
    Narration = narration;
    Region = region;
    SupplierCodePrefix = supplierCodePrefix;
    DateInserted___ = dateInserted___;
    DateUpdated___ = dateUpdated___;
  }

  public string? CostCentreCode { get; }
  public string? Description { get; }
  public string? Narration { get; }
  public string? Region { get; }
  public string? SupplierCodePrefix { get; }
  public DateTime? DateInserted___ { get; }
  public DateTime? DateUpdated___ { get; }
  public bool? IsActive { get; }
}
