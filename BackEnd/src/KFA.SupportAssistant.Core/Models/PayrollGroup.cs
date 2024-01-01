using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_payroll_groups")]
public sealed record class PayrollGroup : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_payroll_groups";
  // [Required]
  [Column("group_id")]
  [Encrypted]
  [Key]
  public override string? Id { get; set; } = string.Empty;

  // [Required]
  [MaxLength(255, ErrorMessage = "Please group name must be 255 characters or less")]
  [Column("group_name")]
  [Encrypted]
  public string? GroupName { get; init; } = string.Empty;

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  [Encrypted]
  public string? Narration { get; init; } = string.Empty;
  public override object ToBaseDTO()
  {
    return (PayrollGroupDTO)this;
  }
}
