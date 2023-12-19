using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_command_details")]
public sealed record class CommandDetail : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_command_details";
  //[Required]
  [MaxLength(25, ErrorMessage = "Please action must be 25 characters or less")]
  [Column("action")]
  public string? Action { get; init; }

  //[Required]
  [MaxLength(10, ErrorMessage = "Please active state must be 10 characters or less")]
  [Column("active_state")]
  public string? ActiveState { get; init; }

  // [Required]
  [MaxLength(25, ErrorMessage = "Please category must be 25 characters or less")]
  [Column("category")]
  public string? Category { get; init; }

  // [Required]
  [Column("command_id")]
  public override string? Id { get; set; }

  // [Required]
  [MaxLength(25, ErrorMessage = "Please command name must be 25 characters or less")]
  [Column("command_name")]
  public string? CommandName { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please command text must be 255 characters or less")]
  [Column("command_text")]
  public string? CommandText { get; init; }

  // [Required]
  [Column("image_id")]
  public long? ImageId { get; init; }

  //[Required]
  [MaxLength(255, ErrorMessage = "Please image path must be 255 characters or less")]
  [Column("image_path")]
  public string? ImagePath { get; init; }

  // [Required]
  [Column("is_enabled")]
  public bool? IsEnabled { get; init; }

  // [Required]
  [Column("is_published")]
  public bool? IsPublished { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  //[Required]
  [MaxLength(255, ErrorMessage = "Please shortcut key must be 255 characters or less")]
  [Column("shortcut_key")]
  public string? ShortcutKey { get; init; }

  public ICollection<UserAuditTrail>? UserAuditTrails { get; set; }

  public override object ToBaseDTO()
  {
    return (CommandDetailDTO)this;
  }
}
