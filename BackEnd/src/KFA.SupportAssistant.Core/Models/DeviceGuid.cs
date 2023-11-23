using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.DynamicsAssistant.Infrastructure.Models;
[Table("tbl_device_guids")]
public sealed record class DeviceGuid : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_device_guids";
  [Required]
  [Column("guid")]
  public override string? Id { get; set; }
}
