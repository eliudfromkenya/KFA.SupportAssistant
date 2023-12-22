using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

public class PatchStockCountSheetRequest : JsonPatchDocument<StockCountSheetDTO>, IPlainTextRequest
{
  public const string Route = "/stock_count_sheets/{countSheetId}";

  public static string BuildRoute(string countSheetId) => Route.Replace("{countSheetId}", countSheetId);

  public string CountSheetId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<StockCountSheetDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<StockCountSheetDTO>>(Content)!;
}
