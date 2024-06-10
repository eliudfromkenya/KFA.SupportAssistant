namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

public readonly struct CreateVendorResponse(string? contact, string? descriptions, string? email, bool? isActive, string? vendorCode, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Contact { get; } = contact;
  public string? Descriptions { get; } = descriptions;
  public string? Email { get; } = email;
  public bool? IsActive { get; } = isActive;
  public string? VendorCode { get; } = vendorCode;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
