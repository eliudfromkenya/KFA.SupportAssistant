using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_asset_details")]
public sealed record class ComputerAssetDetail : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_asset_details";
  [Required]
  [Column("asset_id")]
  public override string? Id { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please asset name must be 255 characters or less")]
  [Column("asset_name")]
  public string? AssetName { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [Required]
  [Column("group_id")]
  public string? GroupID { get; init; }

  [ForeignKey(nameof(GroupID))]
  public ComputerAssetGroup? Group { get; set; }
  //[Reactive, NotMapped]
  public string? Group_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please serial number must be 255 characters or less")]
  [Column("serial_number")]
  public string? SerialNumber { get; init; }

  [MaxLength(255, ErrorMessage = "Please state must be 255 characters or less")]
  [Column("state")]
  public string? State { get; init; }

  public ICollection<ComputerAssetAcquisition>? ComputerAssetAcquisitions { get; set; }
  public ICollection<ComputerAssetAssignment>? ComputerAssetAssignments { get; set; }
  public ICollection<ComputerAssetTransfer>? ComputerAssetTransfers { get; set; }
  public ICollection<ComputerAssetValuation>? ComputerAssetValuations { get; set; }
  public ICollection<ComputerAssetsMaintainceAccessory>? ComputerAssetsMaintainceAccessories { get; set; }
  public ICollection<ComputerAssetsMaintainceDispatch>? ComputerAssetsMaintainceDispatches { get; set; }
  public ICollection<ComputerAssetsMaintainceReceiving>? ComputerAssetsMaintainceReceivings { get; set; }
  public ICollection<ComputerAssetsPostsToDynamic>? ComputerAssetsPostsToDynamics { get; set; }
  public ICollection<ComputerAssetsStatus>? ComputerAssetsStatuses { get; set; }
  public ICollection<ComputerAssetsWriteOff>? ComputerAssetsWriteOffs { get; set; }
  public ICollection<ComputerRemoteAddress>? ComputerRemoteAddresses { get; set; }


  public override object ToBaseDTO()
  {
    return(ComputerAssetDetailDTO)this;
  }
}
