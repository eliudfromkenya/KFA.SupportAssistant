using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Infrastructure.Models;
[Table("tbl_verification_types")]
public sealed record class VerificationType : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_verification_types";
  [MaxLength(255, ErrorMessage = "Please category must be 255 characters or less")]
  [Column("category")]
  public string? Category { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("verification_type_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please verification type name must be 255 characters or less")]
  [Column("verification_type_name")]
  public string? VerificationTypeName { get; init; }
}
