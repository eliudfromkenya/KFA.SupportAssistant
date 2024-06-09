using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_assets_posts_to_dynamics")]
public sealed record class ComputerAssetsPostsToDynamic : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_assets_posts_to_dynamics";
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [Column("date_generated")]
  public global::System.DateTime DateGenerated { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [Required]
  [Column("post_id")]
  public override string? Id { get; init; }

  [Required]
  [Column("posted")]
  public bool Posted { get; init; }
  public override object ToBaseDTO()
  {
    return (ComputerAssetsPostsToDynamicDTO)this;
  }
}
