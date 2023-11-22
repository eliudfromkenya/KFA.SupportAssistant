using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.DynamicsAssistant.Infrastructure.Models;
[Table("tbl_user_logins")]
internal sealed record class UserLogin : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_user_logins";
  [Required]
  [MaxLength(100, ErrorMessage = "Please device id must be 100 characters or less")]
  [Column("device_id")]
  public string? DeviceId { get; init; }

  [Required]
  [Column("from_date")]
  public global::System.DateTime FromDate { get; init; }

  [Required]
  [Column("login_id")]
  public override string? Id { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("upto_date")]
  public global::System.DateTime UptoDate { get; init; }

  [Required]
  [Column("user_id")]
  public string? UserId { get; init; }

  [ForeignKey(nameof(UserId))]
  public SystemUser? User { get; set; }

  public string? User_Caption { get; set; }
  public ICollection<UserAuditTrail>? UserAuditTrails { get; set; }
 public ICollection<Verification>? Verifications { get; set; }
}
