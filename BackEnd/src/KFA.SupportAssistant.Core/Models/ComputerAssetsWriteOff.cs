using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_assets_write_offs")]
public sealed record class ComputerAssetsWriteOff : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_assets_write_offs";
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please reason for writeoff must be 255 characters or less")]
  [Column("reason_for_writeoff")]
  public string? ReasonForWriteOff { get; init; }

  [Required]
  [Column("write_off_id")]
  public override string? Id { get; init; }

  [Column("writeoff_date")]
  public global::System.DateTime WriteOffDate { get; init; }

  public override object ToBaseDTO()
  {
    return (ComputerAssetsWriteOffDTO)this;
  }
}
