using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_user_rights")]
public sealed record class UserRight : BaseModel
{
  public override object ToBaseDTO()
  {
    return (UserRightDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_user_rights";
  // [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please object name must be 255 characters or less")]
  [Column("object_name")]
  public string? ObjectName { get; init; }

  [Column("command_id")]
  public string? CommandId { get; init; }

  [ForeignKey(nameof(CommandId))]
  public CommandDetail? CommandDetail { get; set; }
  [NotMapped]
  public string? Command_Caption { get; set; }

  [Column("right_id")]
  public string? RightId { get; init; }

  [ForeignKey(nameof(RightId))]
  public SystemRight? Right { get; set; }
  [NotMapped]
  public string? Right_Caption { get; set; }

  // [Required]
  [Column("role_id")]
  public string? RoleId { get; init; }

  [ForeignKey(nameof(RoleId))]
  public UserRole? Role { get; set; }
  [NotMapped]
  public string? Role_Caption { get; set; }

  // [Required]
  [Column("user_id")]
  public string? UserId { get; init; }

  [ForeignKey(nameof(UserId))]
  public SystemUser? User { get; set; }
  [NotMapped]
  public string? User_Caption { get; set; }

  // [Required]
  [Column("user_right_id")]
  public override string? Id { get; init; }
  [Column("user_activities")]
  public UserActivities UserActivities { get; set; } = UserActivities.None;
}
