using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_purchases_budget_details")]
public sealed record class PurchasesBudgetDetail : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_purchases_budget_details";
  [Column("batch_key")]
  public string? BatchKey { get; init; }

  [Column("buying_price")]
  public decimal BuyingPrice { get; init; }

  [Column("item_code")]
  public string? ItemCode { get; init; }

  [ForeignKey(nameof(ItemCode))]
  public StockItem? Item { get; set; }
  [NotMapped]
  public string? Item_Caption { get; set; }

  [Column("month")]
  public byte Month { get; init; }

  [Column("month_01_quantity")]
  public decimal Month01Quantity { get; init; }

  [Column("month_02__quantity")]
  public decimal Month02Quantity { get; init; }

  [Column("month_03__quantity")]
  public decimal Month03Quantity { get; init; }

  [Column("month_04__quantity")]
  public decimal Month04Quantity { get; init; }

  [Column("month_05__quantity")]
  public decimal Month05Quantity { get; init; }

  [Column("month_06__quantity")]
  public decimal Month06Quantity { get; init; }

  [Column("month_07__quantity")]
  public decimal Month07Quantity { get; init; }

  [Column("month_08__quantity")]
  public decimal Month08Quantity { get; init; }

  [Column("month_09__quantity")]
  public decimal Month09Quantity { get; init; }

  [Column("month_10__quantity")]
  public decimal Month10Quantity { get; init; }

  [Column("month_11__quantity")]
  public decimal Month11Quantity { get; init; }

  [Column("month_12__quantity")]
  public decimal Month12Quantity { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  //    [Index(IsUnique = true)]
  [Column("purchases_budget_detail_id")]
  public override string? Id { get; init; }

  [Column("unit_cost_price")]
  public decimal UnitCostPrice { get; init; }

  public override object ToBaseDTO()
  {
    return (PurchasesBudgetDetailDTO)this;
  }
}
