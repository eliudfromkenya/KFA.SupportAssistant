namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

public record DeleteVendorRequest
{
  public const string Route = "/vendors/{vendorCode}";
  public static string BuildRoute(string? vendorCode) => Route.Replace("{vendorCode}", vendorCode);
  public string? VendorCode { get; set; }
}
