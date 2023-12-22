namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

public readonly struct CreateVendorCodesRequestResponse(string? attandedBy, string? costCentreCode, string? description, string? narration, string? requestingUser, string? status, DateTime? timeAttended, DateTime? TimeOfRequest, string? vendorCode, string? vendorCodeRequestID, string? vendorType, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AttandedBy { get; } = attandedBy;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Description { get; } = description;
  public string? Narration { get; } = narration;
  public string? RequestingUser { get; } = requestingUser;
  public string? Status { get; } = status;
  public DateTime? TimeAttended { get; } = timeAttended;
  public DateTime? TimeOfRequest { get; } = TimeOfRequest;
  public string? VendorCode { get; } = vendorCode;
  public string? VendorCodeRequestID { get; } = vendorCodeRequestID;
  public string? VendorType { get; } = vendorType;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
