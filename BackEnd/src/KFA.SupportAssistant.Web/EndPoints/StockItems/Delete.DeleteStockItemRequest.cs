namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

public record DeleteStockItemRequest
{
  public const string Route = "/stock_items/{itemCode}";
  public static string BuildRoute(string? itemCode) => Route.Replace("{itemCode}", itemCode);
  public string? ItemCode { get; set; }
}
