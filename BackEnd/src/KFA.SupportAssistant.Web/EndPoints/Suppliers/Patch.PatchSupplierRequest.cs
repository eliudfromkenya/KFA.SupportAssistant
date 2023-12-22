using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

public class PatchSupplierRequest : JsonPatchDocument<SupplierDTO>, IPlainTextRequest
{
  public const string Route = "/suppliers/{supplierId}";

  public static string BuildRoute(string supplierId) => Route.Replace("{supplierId}", supplierId);

  public string SupplierId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<SupplierDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<SupplierDTO>>(Content)!;
}
