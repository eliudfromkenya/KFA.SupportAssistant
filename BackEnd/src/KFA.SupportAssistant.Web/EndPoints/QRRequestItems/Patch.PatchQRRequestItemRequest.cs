using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

public class PatchQRRequestItemRequest : JsonPatchDocument<QRRequestItemDTO>, IPlainTextRequest
{
  public const string Route = "/qr_request_items/{saleID}";

  public static string BuildRoute(string saleID) => Route.Replace("{saleID}", saleID);

  public string SaleID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<QRRequestItemDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<QRRequestItemDTO>>(Content)!;
}
