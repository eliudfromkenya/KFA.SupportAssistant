using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Infrastructure.Models;
[Table("tbl_qr_request_items")]
public sealed record class QRRequestItem : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_qr_request_items";
  [MaxLength(30, ErrorMessage = "Please cash sale number must be 30 characters or less")]
  [Column("cash_sale_number")]
  public string? CashSaleNumber { get; init; }

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }

  public string? CostCentre_Caption { get; set; }

  [MaxLength(25, ErrorMessage = "Please hs code must be 25 characters or less")]
  [Column("hs_code")]
  public string? HsCode { get; init; }

  [MaxLength(255, ErrorMessage = "Please hs name must be 255 characters or less")]
  [Column("hs_name")]
  public string? HsName { get; init; }

  [MaxLength(16, ErrorMessage = "Please item code must be 16 characters or less")]
  [Column("item_code")]
  public string? ItemCode { get; init; }

  [MaxLength(255, ErrorMessage = "Please item name must be 255 characters or less")]
  [Column("item_name")]
  public string? ItemName { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Column("percentage_discount")]
  public decimal PercentageDiscount { get; init; }

  [Column("quantity")]
  public decimal Quantity { get; init; }

  [Column("request_id")]
  public string? RequestID { get; init; }

  [ForeignKey(nameof(RequestID))]
  public QRCodesRequest? QRCodesRequest { get; set; }

  [Required]
  [Column("sale_id")]
  public override string? Id { get; set; }

  [Column("time")]
  public global::System.DateTime Time { get; init; }

  [Column("total_amount")]
  public decimal TotalAmount { get; init; }

  [MaxLength(25, ErrorMessage = "Please unit of measure must be 25 characters or less")]
  [Column("unit_of_measure")]
  public string? UnitOfMeasure { get; init; }

  [Column("unit_price")]
  public decimal UnitPrice { get; init; }

  [Column("vat_amount")]
  public decimal VATAmount { get; init; }

  [MaxLength(4, ErrorMessage = "Please vat class must be 4 characters or less")]
  [Column("vat_class")]
  public string? VATClass { get; init; }
}
