using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_device_guids")]
public sealed record class DeviceGuid : BaseModel
{
  public override object ToBaseDTO()
  {
    return (DeviceGuidDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_device_guids";
  // [Required]
  [Column("guid")]
  public override string? Id { get; init; }
}
