using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_assets_maintaince_receivings")]
public sealed record class ComputerAssetsMaintainceReceiving : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_assets_maintaince_receivings";
  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please brought by must be 255 characters or less")]
  [Column("brought_by")]
  public string? BroughtBy { get; init; }

  [Column("date_recieved")]
  public global::System.DateTime DateRecieved { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [Column("from_department_code")]
  public string? FromDepartmentCode { get; init; }

  [ForeignKey(nameof(FromDepartmentCode))]
  public CostCentre? FromDepartment { get; set; }
  //[Reactive, NotMapped]
  public string? FromDepartment_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please reason to send must be 255 characters or less")]
  [Column("reason_to_send")]
  public string? ReasonToSend { get; init; }

  [Required]
  [Column("receive_id")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please recieved by must be 255 characters or less")]
  [Column("recieved_by")]
  public string? RecievedBy { get; init; }

  public ICollection<ComputerAssetMaintaince>? ComputerAssetMaintainces { get; set; }

  public override object ToBaseDTO()
  {
    return (ComputerAssetsMaintainceReceivingDTO)this;
  }
}
