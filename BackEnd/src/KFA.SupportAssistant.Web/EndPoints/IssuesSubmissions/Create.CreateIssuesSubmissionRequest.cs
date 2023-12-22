using System.ComponentModel.DataAnnotations;
using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

public class CreateIssuesSubmissionRequest
{
  public const string Route = "/issues_submissions";

  [Required]
  public string? IssueID { get; set; }

  public string? Narration { get; set; }
  public IssueStatus? Status { get; set; }

  [Required]
  public string? SubmissionID { get; set; }

  [Required]
  public string? SubmittedTo { get; set; }

  [Required]
  public string? SubmittingUser { get; set; }

  public DateTime? TimeSubmitted { get; set; }
  public string? Type { get; set; }
}
