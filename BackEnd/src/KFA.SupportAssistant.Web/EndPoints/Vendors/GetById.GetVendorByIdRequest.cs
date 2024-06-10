namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

public class GetVendorByIdRequest
{
  public const string Route = "/vendors/{vendorCode}";

  public static string BuildRoute(string? vendorCode) => Route.Replace("{vendorCode}", vendorCode);

  public string? VendorCode { get; set; }
}
