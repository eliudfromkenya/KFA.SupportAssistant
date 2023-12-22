namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

public readonly struct CreateSupplierResponse(string? address, string? contactPerson, string? costCentreCode, string? description, string? email, string? narration, string? postalCode, string? supplierCode, string? supplierId, string? telephone, string? town, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Address { get; } = address;
  public string? ContactPerson { get; } = contactPerson;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Description { get; } = description;
  public string? Email { get; } = email;
  public string? Narration { get; } = narration;
  public string? PostalCode { get; } = postalCode;
  public string? SupplierCode { get; } = supplierCode;
  public string? SupplierId { get; } = supplierId;
  public string? Telephone { get; } = telephone;
  public string? Town { get; } = town;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
