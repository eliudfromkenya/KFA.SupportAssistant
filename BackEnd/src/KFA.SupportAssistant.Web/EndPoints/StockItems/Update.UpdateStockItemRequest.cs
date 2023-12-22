using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

public record UpdateStockItemRequest
{
  public const string Route = "/stock_items/{itemCode}";
  public string? Barcode { get; set; }
  public string? GroupId { get; set; }
  [Required]
  public string? ItemCode { get; set; }
  [Required]
  public string? ItemName { get; set; }
  public string? Narration { get; set; }
}
