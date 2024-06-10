using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_assets_statuses")]
public sealed record class ComputerAssetsStatus : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_assets_statuses";
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [Required]
  [Column("assets_status_id")]
  public override string? Id { get; init; }

  [Column("date")]
  public global::System.DateTime Date { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(255, ErrorMessage = "Please done by must be 255 characters or less")]
  [Column("done_by")]
  public string? DoneBy { get; init; }

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public string? Status { get; init; }

  public override object ToBaseDTO()
  {
    return (ComputerAssetsStatusDTO)this;
  }
}
