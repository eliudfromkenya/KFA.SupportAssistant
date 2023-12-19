using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_payroll_groups")]
public sealed record class PayrollGroup : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_payroll_groups";
  [Required]
  [Column("group_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please group name must be 255 characters or less")]
  [Column("group_name")]
  public string? GroupName { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }
  public override object ToBaseDTO()
  {
    return (PayrollGroupDTO)this;
  }
}
