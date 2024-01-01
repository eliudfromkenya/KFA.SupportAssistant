using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_password_safes")]
public sealed record class PasswordSafe : BaseModel
{
  public override object ToBaseDTO()
  {
    return (PasswordSafeDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_password_safes";
  // [Required]
  [Encrypted]
  [MaxLength(255, ErrorMessage = "Please details must be 255 characters or less")]
  [Column("details")]
  public string? Details { get; init; } = string.Empty;

  // [Required]
  [Encrypted]
  [MaxLength(255, ErrorMessage = "Please name must be 255 characters or less")]
  [Column("name")]
  public string? Name { get; init; } = string.Empty;

  // [Required]
  [Encrypted]
  [MaxLength(255, ErrorMessage = "Please password must be 255 characters or less")]
  [Column("password")]
  public string? Password { get; init; } = string.Empty;

  // [Required]
  [Column("password_id")]
  public override string? Id { get; set; }

  [MaxLength(255, ErrorMessage = "Please reminder must be 255 characters or less")]
  [Column("reminder")]
  [Encrypted]
  public string? Reminder { get; init; } = string.Empty;

  [Column("users_visible_to")]
  [Encrypted]
  public string? UsersVisibleTo { get; init; } = string.Empty;
}
