using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

public class PatchVendorCodesRequestRequest : JsonPatchDocument<VendorCodesRequestDTO>, IPlainTextRequest
{
  public const string Route = "/vendor_codes_requests/{vendorCodeRequestID}";

  public static string BuildRoute(string vendorCodeRequestID) => Route.Replace("{vendorCodeRequestID}", vendorCodeRequestID);

  public string VendorCodeRequestID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<VendorCodesRequestDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<VendorCodesRequestDTO>>(Content)!;
}
