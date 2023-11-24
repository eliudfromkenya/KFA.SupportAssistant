using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Infrastructure.Models;
[Table("tbl_user_rights")]
public sealed record class UserRight : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_user_rights";
  [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please object name must be 255 characters or less")]
  [Column("object_name")]
  public string? ObjectName { get; init; }

  [Column("command_id")]
  public string? CommandId { get; init; }

  [ForeignKey(nameof(CommandId))]
  public CommandDetail? CommandDetail { get; set; }

  public string? Command_Caption { get; set; }

  [Column("right_id")]
  public string? RightId { get; init; }

  [ForeignKey(nameof(RightId))]
  public SystemRight? Right { get; set; }

  public string? Right_Caption { get; set; }

  [Required]
  [Column("role_id")]
  public string? RoleId { get; init; }

  [ForeignKey(nameof(RoleId))]
  public UserRole? Role { get; set; }

  public string? Role_Caption { get; set; }

  [Required]
  [Column("user_id")]
  public string? UserId { get; init; }

  [ForeignKey(nameof(UserId))]
  public SystemUser? User { get; set; }

  public string? User_Caption { get; set; }

  [Required]
  [Column("user_right_id")]
  public override string? Id { get; set; }
}
