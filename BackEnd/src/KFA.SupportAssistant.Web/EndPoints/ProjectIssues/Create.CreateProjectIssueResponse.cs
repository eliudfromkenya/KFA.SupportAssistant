using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

public readonly struct CreateProjectIssueResponse(string? category, global::System.DateTime? date, string? description, string? effect, string? narration, string? projectIssueID, string? registeredBy, IssueStatus? status, string? subCategory, string? title, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Category { get; } = category;
  public global::System.DateTime? Date { get; } = date;
  public string? Description { get; } = description;
  public string? Effect { get; } = effect;
  public string? Narration { get; } = narration;
  public string? ProjectIssueID { get; } = projectIssueID;
  public string? RegisteredBy { get; } = registeredBy;
  public IssueStatus? Status { get; } = status;
  public string? SubCategory { get; } = subCategory;
  public string? Title { get; } = title;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
