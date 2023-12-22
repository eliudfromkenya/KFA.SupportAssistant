namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

public record DeleteVendorCodesRequestRequest
{
  public const string Route = "/vendor_codes_requests/{vendorCodeRequestID}";
  public static string BuildRoute(string? vendorCodeRequestID) => Route.Replace("{vendorCodeRequestID}", vendorCodeRequestID);
  public string? VendorCodeRequestID { get; set; }
}
