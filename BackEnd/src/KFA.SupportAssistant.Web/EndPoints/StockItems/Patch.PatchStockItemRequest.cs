using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

public class PatchStockItemRequest : JsonPatchDocument<StockItemDTO>, IPlainTextRequest
{
  public const string Route = "/stock_items/{itemCode}";

  public static string BuildRoute(string itemCode) => Route.Replace("{itemCode}", itemCode);

  public string ItemCode { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<StockItemDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<StockItemDTO>>(Content)!;
}
