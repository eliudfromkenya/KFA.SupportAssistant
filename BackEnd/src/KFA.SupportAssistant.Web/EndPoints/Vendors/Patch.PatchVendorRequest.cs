using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

public class PatchVendorRequest : JsonPatchDocument<VendorDTO>, IPlainTextRequest
{
  public const string Route = "/vendors/{vendorCode}";

  public static string BuildRoute(string vendorCode) => Route.Replace("{vendorCode}", vendorCode);

  public string VendorCode { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<VendorDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<VendorDTO>>(Content)!;
}
