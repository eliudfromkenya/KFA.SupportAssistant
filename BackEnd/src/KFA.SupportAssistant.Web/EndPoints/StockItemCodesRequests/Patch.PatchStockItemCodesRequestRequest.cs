using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

public class PatchStockItemCodesRequestRequest : JsonPatchDocument<StockItemCodesRequestDTO>, IPlainTextRequest
{
  public const string Route = "/stock_item_codes_requests/{itemCodeRequestID}";

  public static string BuildRoute(string itemCodeRequestID) => Route.Replace("{itemCodeRequestID}", itemCodeRequestID);

  public string ItemCodeRequestID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<StockItemCodesRequestDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<StockItemCodesRequestDTO>>(Content)!;
}
