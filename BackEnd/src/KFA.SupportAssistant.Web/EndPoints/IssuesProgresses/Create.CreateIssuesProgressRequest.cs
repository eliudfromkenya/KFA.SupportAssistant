using System.ComponentModel.DataAnnotations;
using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

public class CreateIssuesProgressRequest
{
  public const string Route = "/issues_progresses";

  [Required]
  public string? Description { get; set; }

  [Required]
  public string? IssueID { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? ProgressID { get; set; }

  public string? ReportedBy { get; set; }
  public IssueStatus? Status { get; set; }
  public global::System.DateTime? Time { get; set; }
}
