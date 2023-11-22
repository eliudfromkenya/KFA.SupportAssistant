using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.DynamicsAssistant.Infrastructure.Models;
[Table("tbl_stock_items")]
internal sealed record class StockItem : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_stock_items";
  [MaxLength(255, ErrorMessage = "Please barcode must be 255 characters or less")]
  [Column("barcode")]
  public string? Barcode { get; init; }

  [Column("group_id")]
  public string? GroupId { get; init; }

  [ForeignKey(nameof(GroupId))]
  public ItemGroup? Group { get; set; }

  public string? Group_Caption { get; set; }

  [Required]
  [Column("item_code")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please item name must be 255 characters or less")]
  [Column("item_name")]
  public string? ItemName { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }
}
