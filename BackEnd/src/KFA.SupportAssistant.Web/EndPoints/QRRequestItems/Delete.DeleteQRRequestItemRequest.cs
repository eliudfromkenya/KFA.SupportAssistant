namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

public record DeleteQRRequestItemRequest
{
  public const string Route = "/qr_request_items/{saleID}";
  public static string BuildRoute(string? saleID) => Route.Replace("{saleID}", saleID);
  public string? SaleID { get; set; }
}
