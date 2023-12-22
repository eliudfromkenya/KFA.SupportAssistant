namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

public record DeleteStockItemCodesRequestRequest
{
  public const string Route = "/stock_item_codes_requests/{itemCodeRequestID}";
  public static string BuildRoute(string? itemCodeRequestID) => Route.Replace("{itemCodeRequestID}", itemCodeRequestID);
  public string? ItemCodeRequestID { get; set; }
}
