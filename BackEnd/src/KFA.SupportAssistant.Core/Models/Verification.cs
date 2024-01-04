using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_verifications")]
public sealed record class Verification : BaseModel
{
  public override object ToBaseDTO()
  {
    return (VerificationDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_verifications";
  // [Required]
  [Column("date_of_verification")]
  public global::System.DateTime DateOfVerification { get; init; }

  // [Required]
  [Column("login_id")]
  public string? LoginId { get; init; }

  [ForeignKey(nameof(LoginId))]
  public UserLogin? Login { get; set; }
  [NotMapped]
  public string? Login_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  // [Required]
  [Column("record_id")]
  public long RecordId { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please table name must be 255 characters or less")]
  [Column("table_name")]
  public string? TableName { get; init; }

  // [Required]
  [Column("verification_id")]
  public override string? Id { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please verification name must be 255 characters or less")]
  [Column("verification_name")]
  public string? VerificationName { get; init; }

  [Column("verification_record_id")]
  public long VerificationRecordId { get; init; }

  // [Required]
  [Column("verification_type_id")]
  public long VerificationTypeId { get; init; }
}
