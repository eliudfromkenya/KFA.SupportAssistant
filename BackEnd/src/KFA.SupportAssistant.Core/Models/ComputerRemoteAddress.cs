using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_remote_addresses")]
public sealed record class ComputerRemoteAddress : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_remote_addresses";
  [Required]
  [Column("anydesk_id")]
  public override string? Id { get; init; }

  [Required]
  //    [Index(IsUnique = true)]
  [MaxLength(100, ErrorMessage = "Please anydesk number must be 100 characters or less")]
  [Column("anydesk_number")]
  public string? AnyDeskNumber { get; init; }

  [Required]
  [MaxLength(40, ErrorMessage = "Please anydesk password must be 40 characters or less")]
  [Column("anydesk_password")]
  public string? AnydeskPassword { get; init; }

  [Column("asset_detail_id")]
  public string? AssetDetailID { get; init; }

  [ForeignKey(nameof(AssetDetailID))]
  public ComputerAssetDetail? AssetDetail { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDetail_Caption { get; set; }

  [Required]
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  //[Reactive, NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Required]
  [MaxLength(25, ErrorMessage = "Please device name must be 25 characters or less")]
  [Column("device_name")]
  public string? DeviceName { get; init; }

  [Required]
  [MaxLength(100, ErrorMessage = "Please name of user must be 100 characters or less")]
  [Column("name_of_user")]
  public string? NameOfUser { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please team viewer address must be 255 characters or less")]
  [Column("team_viewer_address")]
  public string? TeamViewerAddress { get; init; }

  [MaxLength(255, ErrorMessage = "Please type must be 255 characters or less")]
  [Column("type")]
  public string? Type { get; init; }

  public override object ToBaseDTO()
  {
    return(ComputerRemoteAddressDTO)this;
  }
}
