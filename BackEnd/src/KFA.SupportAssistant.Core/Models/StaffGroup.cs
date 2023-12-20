using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_staff_groups")]
public sealed record class StaffGroup : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_staff_groups";
  // [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  // [Required]
  [Column("group_number")]
  public override string? Id { get; set; }

  // [Required]
  [Column("is_active")]
  public bool IsActive { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  public ICollection<EmployeeDetail>? EmployeeDetails { get; set; }
  public override object ToBaseDTO()
  {
    return (StaffGroupDTO)this;
  }
}
