using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;

[Table("tbl_data_devices")]
public sealed record class DataDevice : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_data_devices";
  [MaxLength(255, ErrorMessage = "Please device caption must be 255 characters or less")]
  [Column("device_caption")]
  public string? DeviceCaption { get; init; }

  [Required]
  [MaxLength(100, ErrorMessage = "Please device code must be 100 characters or less")]
  [Column("device_code")]
  public string? DeviceCode { get; init; }

  [Required]
  [Column("device_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please device name must be 255 characters or less")]
  [Column("device_name")]
  public string? DeviceName { get; init; }

  [MaxLength(255, ErrorMessage = "Please device number must be 255 characters or less")]
  [Column("device_number")]
  public string? DeviceNumber { get; init; }

  [MaxLength(255, ErrorMessage = "Please device right must be 255 characters or less")]
  [Column("device_right")]
  public string? DeviceRight { get; init; }

  [Required]
  [Column("station_id")]
  public string? StationID { get; init; }

  [ForeignKey(nameof(StationID))]
  public CostCentre? Station { get; set; }

  public string? Station_Caption { get; set; }


  [MaxLength(255, ErrorMessage = "Please type of device must be 255 characters or less")]
  [Column("type_of_device")]
  public string? TypeOfDevice { get; init; }

  public ICollection<VerificationRight>? VerificationRights { get; set; }
}
