using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models.Types;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_issues_submissions")]
public sealed record class IssuesSubmission : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_issues_submissions";
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

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public IssueStatus? Status { get; init; }

  // [Required]
  [Column("submission_id")]
  public override string? Id { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please submitted to must be 255 characters or less")]
  [Column("submitted_to")]
  public string? SubmittedTo { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please submitting user must be 255 characters or less")]
  [Column("submitting_user")]
  public string? SubmittingUser { get; init; }

  [MaxLength(255, ErrorMessage = "Please time submitted must be 255 characters or less")]
  [Column("time_submitted")]
  public DateTime? TimeSubmitted { get; init; }

  [MaxLength(255, ErrorMessage = "Please type must be 255 characters or less")]
  [Column("type")]
  public string? Type { get; init; }

  public override object ToBaseDTO()
  {
    return (IssuesSubmissionDTO)this;
  }
}
