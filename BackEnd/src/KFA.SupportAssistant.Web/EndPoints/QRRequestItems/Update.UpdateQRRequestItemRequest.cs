using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

public record UpdateQRRequestItemRequest
{
  public const string Route = "/qr_request_items/{saleID}";
  public string? CashSaleNumber { get; set; }
  public string? CostCentreCode { get; set; }
  public string? HsCode { get; set; }
  public string? HsName { get; set; }
  public string? ItemCode { get; set; }
  public string? ItemName { get; set; }
  public string? Narration { get; set; }
  public decimal? PercentageDiscount { get; set; }
  public decimal? Quantity { get; set; }
  public string? RequestID { get; set; }
  [Required]
  public string? SaleID { get; set; }
  public global::System.DateTime? Time { get; set; }
  public decimal? TotalAmount { get; set; }
  public string? UnitOfMeasure { get; set; }
  public decimal? UnitPrice { get; set; }
  public decimal? VATAmount { get; set; }
  public string? VATClass { get; set; }
}
