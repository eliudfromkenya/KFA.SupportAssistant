using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Infrastructure.Models;
[Table("tbl_verification_rights")]
public sealed record class VerificationRight : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_verification_rights";
  [Column("device_id")]
  public string? DeviceId { get; init; }

  [ForeignKey(nameof(DeviceId))]
  public DataDevice? Device { get; set; }

  public string? Device_Caption { get; set; }


  [Column("user_id")]
  public string? UserId { get; init; }

  [ForeignKey(nameof(UserId))]
  public SystemUser? User { get; set; }

  public string? User_Caption { get; set; }


  [Required]
  [Column("user_role_id")]
  public string? UserRoleId { get; init; }

  [ForeignKey(nameof(UserRoleId))]
  public UserRole? UserRole { get; set; }

  public string? UserRole_Caption { get; set; }


  [Required]
  [Column("verification_right_id")]
  public override string? Id { get; set; }

  [Column("verification_type_id")]
  public long VerificationTypeId { get; init; }
}
