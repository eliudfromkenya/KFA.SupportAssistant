namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

public class GetStockItemByIdRequest
{
  public const string Route = "/stock_items/{itemCode}";

  public static string BuildRoute(string? itemCode) => Route.Replace("{itemCode}", itemCode);

  public string? ItemCode { get; set; }
}
