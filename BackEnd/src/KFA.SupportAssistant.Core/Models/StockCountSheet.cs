using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_stock_count_sheets")]
public sealed record class StockCountSheet : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_stock_count_sheets";
  [Column("actual")]
  public decimal Actual { get; init; }

  [Column("average_age_months")]
  public decimal AverageAgeMonths { get; init; }

  [Column("batch_key")]
  public string? BatchKey { get; init; }

  [Required]
  [Column("count_sheet_document_id")]
  public long CountSheetDocumentId { get; init; }

  [Required]
  [Column("count_sheet_id")]
  public override string? Id { get; set; }

  [MaxLength(10, ErrorMessage = "Please document number must be 10 characters or less")]
  [Column("document_number")]
  public string? DocumentNumber { get; init; }

  [Required]
  [Column("item_code")]
  public string? ItemCode { get; init; }

  [ForeignKey(nameof(ItemCode))]
  public StockItem? Item { get; set; }
  [NotMapped]
  public string? Item_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Column("quantity_on_hand")]
  public decimal QuantityOnHand { get; init; }

  [Column("quantity_sold_last_12_months")]
  public decimal QuantitySoldLast12Months { get; init; }

  [Column("selling_price")]
  public decimal SellingPrice { get; init; }

  [Column("stocks_over")]
  public decimal StocksOver { get; init; }

  [Column("stocks_short")]
  public decimal StocksShort { get; init; }

  [Column("unitcostprice")]
  public decimal UnitCostPrice { get; init; }

  public override object ToBaseDTO()
  {
    return (StockCountSheetDTO)this;
  }
}
