using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models.Types;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_issues_progresses")]
public sealed record class IssuesProgress : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_issues_progresses";
  // [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  // [Required]
  [Column("issue_id")]
  public string? IssueID { get; init; }

  [ForeignKey(nameof(IssueID))]
  public ProjectIssue? Issue { get; set; }
  [NotMapped]
  public string? Issue_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  // [Required]
  [Column("progress_id")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please reported by must be 255 characters or less")]
  [Column("reported_by")]
  public string? ReportedBy { get; init; }

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public IssueStatus? Status { get; init; }

  [Column("time")]
  public global::System.DateTime Time { get; init; }
  public override object ToBaseDTO()
  {
    return (IssuesProgressDTO)this;
  }
}
