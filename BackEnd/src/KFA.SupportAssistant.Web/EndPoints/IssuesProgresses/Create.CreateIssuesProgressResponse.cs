using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

public readonly struct CreateIssuesProgressResponse(string? description, string? issueID, string? narration, string? progressID, string? reportedBy, IssueStatus? status, global::System.DateTime? time, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Description { get; } = description;
  public string? IssueID { get; } = issueID;
  public string? Narration { get; } = narration;
  public string? ProgressID { get; } = progressID;
  public string? ReportedBy { get; } = reportedBy;
  public IssueStatus? Status { get; } = status;
  public global::System.DateTime? Time { get; } = time;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
