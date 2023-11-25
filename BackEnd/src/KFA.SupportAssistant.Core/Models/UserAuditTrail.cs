using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_user_audit_trails")]
public sealed record class UserAuditTrail : BaseModel
{
  public override object ToBaseDTO()
  {
    return (UserAuditTrailDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_user_audit_trails";
  [Required]
  [Column("activity_date")]
  public global::System.DateTime ActivityDate { get; init; }

  [Required]
  [Column("activity_enum_number")]
  public UserActivities ActivityEnumNumber { get; init; }

  [Required]
  [Column("audit_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please category must be 255 characters or less")]
  [Column("category")]
  public string? Category { get; init; }

  [Required]
  [Column("command_id")]
  public string? CommandId { get; init; }

  [ForeignKey(nameof(CommandId))]
  public CommandDetail? Command { get; set; }

  public string? Command_Caption { get; set; }

  [Required]
  [Column("data")]
  public string? Data { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [Required]
  [Column("login_id")]
  public string? LoginId { get; init; }

  [ForeignKey(nameof(LoginId))]
  public UserLogin? Login { get; set; }

  public string? Login_Caption { get; set; }
  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("old_values")]
  public string? OldValues { get; init; }
}
