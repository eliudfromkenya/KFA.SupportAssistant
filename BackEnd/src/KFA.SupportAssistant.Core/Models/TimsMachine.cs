using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_tims_machines")]
public sealed record class TimsMachine : BaseModel
{
  public override object ToBaseDTO()
  {
    return (TimsMachineDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_tims_machines";
  [MaxLength(5, ErrorMessage = "Please class type must be 5 characters or less")]
  [Column("class_type")]
  public string? ClassType { get; init; }

  [Column("current_status")]
  public byte CurrentStatus { get; init; }

  [MaxLength(255, ErrorMessage = "Please domain name must be 255 characters or less")]
  [Column("domain_name")]
  public string? DomainName { get; init; }

  [MaxLength(20, ErrorMessage = "Please external ip address must be 20 characters or less")]
  [Column("external_ip_address")]
  public string? ExternalIPAddress { get; init; }

  [MaxLength(8, ErrorMessage = "Please external port number must be 8 characters or less")]
  [Column("external_port_number")]
  public string? ExternalPortNumber { get; init; }

  [Required]
  [MaxLength(20, ErrorMessage = "Please internal ip address must be 20 characters or less")]
  [Column("internal_ip_address")]
  public string? InternalIPAddress { get; init; }

  [MaxLength(8, ErrorMessage = "Please internal port number must be 8 characters or less")]
  [Column("internal_port_number")]
  public string? InternalPortNumber { get; init; }

  [Required]
  [Column("machine_id")]
  public override string? Id { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("ready_for_use")]
  public bool ReadyForUse { get; init; }

  [MaxLength(25, ErrorMessage = "Please serial number must be 25 characters or less")]
  [Column("serial_number")]
  public string? SerialNumber { get; init; }

  [MaxLength(255, ErrorMessage = "Please tims name must be 255 characters or less")]
  [Column("tims_name")]
  public string? TimsName { get; init; }
}
