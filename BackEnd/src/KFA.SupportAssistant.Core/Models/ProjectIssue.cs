using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models.Types;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_project_issues")]
public sealed record class ProjectIssue : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_project_issues";
  [MaxLength(255, ErrorMessage = "Please category must be 255 characters or less")]
  [Column("category")]
  public string? Category { get; init; }

  [Column("date")]
  public global::System.DateTime Date { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(255, ErrorMessage = "Please effect must be 255 characters or less")]
  [Column("effect")]
  public string? Effect { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  // [Required]
  [Column("project_issue_id")]
  public override string? Id { get; init; }

  [MaxLength(255, ErrorMessage = "Please registered by must be 255 characters or less")]
  [Column("registered_by")]
  public string? RegisteredBy { get; init; }

  [Column("status")]
  public IssueStatus Status { get; init; }

  [MaxLength(255, ErrorMessage = "Please sub category must be 255 characters or less")]
  [Column("sub_category")]
  public string? SubCategory { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please title must be 255 characters or less")]
  [Column("title")]
  public string? Title { get; init; }

  public ICollection<IssuesAttachment>? IssuesAttachments { get; set; }
  public ICollection<IssuesProgress>? IssuesProgresses { get; set; }
  public ICollection<IssuesSubmission>? IssuesSubmissions { get; set; }
  public override object ToBaseDTO()
  {
    return (ProjectIssueDTO)this;
  }
}
