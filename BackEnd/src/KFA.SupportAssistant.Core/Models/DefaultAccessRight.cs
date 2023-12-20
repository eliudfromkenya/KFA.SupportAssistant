using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_default_access_rights")]
public sealed record class DefaultAccessRight : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_default_access_rights";
  // [Required]
  [Column("right_id")]
  public override string? Id { get; set; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please name must be 255 characters or less")]
  [Column("name")]
  public string? Name { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please type must be 255 characters or less")]
  [Column("type")]
  public string? Type { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please rights must be 255 characters or less")]
  [Column("rights")]
  public string? Rights { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }
  public override object ToBaseDTO()
  {
    return (DefaultAccessRight)this;
  }
}
