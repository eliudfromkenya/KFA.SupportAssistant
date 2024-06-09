using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_asset_transfers")]
public sealed record class ComputerAssetTransfer : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_asset_transfers";
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  //[Reactive, NotMapped]
  public string? CostCentre_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please payroll number must be 255 characters or less")]
  [Column("payroll_number")]
  public string? PayrollNumber { get; init; }

  [MaxLength(255, ErrorMessage = "Please responsible user must be 255 characters or less")]
  [Column("responsible_user")]
  public string? ResponsibleUser { get; init; }

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public string? Status { get; init; }

  [Column("transfer_date")]
  public global::System.DateTime TransferDate { get; init; }

  [Required]
  [Column("transfer_id")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please transfer reasons must be 255 characters or less")]
  [Column("transfer_reasons")]
  public string? TransferReasons { get; init; }

  public override object ToBaseDTO()
  {
    return(ComputerAssetTransferDTO)this;
  }
}
