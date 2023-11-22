using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.DynamicsAssistant.Infrastructure.Models;
[Table("tbl_password_safes")]
internal sealed record class PasswordSafe : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_password_safes";
  [Required]
  [MaxLength(255, ErrorMessage = "Please details must be 255 characters or less")]
  [Column("details")]
  public string? Details { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please name must be 255 characters or less")]
  [Column("name")]
  public string? Name { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please password must be 255 characters or less")]
  [Column("password")]
  public string? Password { get; init; }

  [Required]
  [Column("password_id")]
  public override string? Id { get; set; }

  [MaxLength(255, ErrorMessage = "Please reminder must be 255 characters or less")]
  [Column("reminder")]
  public string? Reminder { get; init; }

  [Column("users_visible_to")]
  public string? UsersVisibleTo { get; init; }
}
