using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_anydesks")]
public sealed record class ComputerAnydesk : BaseModel
{
  public override object ToBaseDTO()
  {
    return (ComputerAnydeskDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_anydesks";
  [Required]
  [Column("anydesk_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(100, ErrorMessage = "Please anydesk number must be 100 characters or less")]
  [Column("anydesk_number")]
  public string? AnyDeskNumber { get; init; }
  [MaxLength(100, ErrorMessage = "Please password must be 100 characters or less")]
  [Column("password")]
  [Encrypted]
  public string? Password { get; init; }

  [MaxLength(100, ErrorMessage = "Please name of user must be 100 characters or less")]
  [Column("name_of_user")]
  public string? NameOfUser { get; init; }

  [Required]
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please device name must be 255 characters or less")]
  [Column("device_name")]
  public string? DeviceName { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please type must be 255 characters or less")]
  [Column("type")]
  public AnyDeskComputerType? Type { get; init; }
}
