using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

public class CreateProjectIssueRequest
{
  public const string Route = "/project_issues";
  public string? Category { get; set; }
  public global::System.DateTime? Date { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? Effect { get; set; }
  public string? Narration { get; set; }

  [Required]
  public string? ProjectIssueID { get; set; }

  public string? RegisteredBy { get; set; }
  public byte? Status { get; set; }
  public string? SubCategory { get; set; }

  [Required]
  public string? Title { get; set; }
}
