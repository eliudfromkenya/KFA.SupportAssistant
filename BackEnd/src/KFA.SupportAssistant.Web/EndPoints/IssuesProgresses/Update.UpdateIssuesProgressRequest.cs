using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

public record UpdateIssuesProgressRequest
{
  public const string Route = "/issues_progresses/{progressID}";
  [Required]
  public string? Description { get; set; }
  [Required]
  public string? IssueID { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? ProgressID { get; set; }
  public string? ReportedBy { get; set; }
  public string? Status { get; set; }
  public global::System.DateTime? Time { get; set; }
}
