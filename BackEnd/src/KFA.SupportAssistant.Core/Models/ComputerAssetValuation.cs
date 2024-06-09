using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_asset_valuations")]
public sealed record class ComputerAssetValuation : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_asset_valuations";
  [Required]
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [Required]
  [Column("revaluation_id")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public string? Status { get; init; }

  [Column("valuation_date")]
  public global::System.DateTime ValuationDate { get; init; }

  [Required]
  [Column("value")]
  public decimal Value { get; init; }
  public override object ToBaseDTO()
  {
    return(ComputerAssetValuationDTO)this;
  }
}
