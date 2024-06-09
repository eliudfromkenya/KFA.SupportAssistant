using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_assets_maintaince_dispatches")]
public sealed record class ComputerAssetsMaintainceDispatch : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_assets_maintaince_dispatches";
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please collected by must be 255 characters or less")]
  [Column("collected_by")]
  public string? CollectedBy { get; init; }

  [Column("date_send")]
  public global::System.DateTime DateSend { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [Required]
  [Column("dispatch_id")]
  public override string? Id { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please reason to send must be 255 characters or less")]
  [Column("reason_to_send")]
  public string? ReasonToSend { get; init; }

  [MaxLength(255, ErrorMessage = "Please sent by must be 255 characters or less")]
  [Column("sent_by")]
  public string? SentBy { get; init; }

  [Column("to_department_code")]
  public string? ToDepartmentCode { get; init; }

  [ForeignKey(nameof(ToDepartmentCode))]
  public CostCentre? ToDepartment { get; set; }
  //[Reactive, NotMapped]
  public string? ToDepartment_Caption { get; set; }

  public ICollection<ComputerAssetMaintaince>? ComputerAssetMaintainces { get; set; }
  
  public override object ToBaseDTO()
  {
    return(ComputerAssetsMaintainceDispatchDTO)this;
  }
}
