namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

public record DeleteStockCountSheetRequest
{
  public const string Route = "/stock_count_sheets/{countSheetId}";
  public static string BuildRoute(string? countSheetId) => Route.Replace("{countSheetId}", countSheetId);
  public string? CountSheetId { get; set; }
}
