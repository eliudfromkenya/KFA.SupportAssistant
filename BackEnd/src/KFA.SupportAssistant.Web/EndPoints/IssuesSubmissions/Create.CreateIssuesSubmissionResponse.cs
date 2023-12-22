using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

public readonly struct CreateIssuesSubmissionResponse(string? issueID, string? narration, IssueStatus? status, string? submissionID, string? submittedTo, string? submittingUser, DateTime? timeSubmitted, string? type, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? IssueID { get; } = issueID;
  public string? Narration { get; } = narration;
  public IssueStatus? Status { get; } = status;
  public string? SubmissionID { get; } = submissionID;
  public string? SubmittedTo { get; } = submittedTo;
  public string? SubmittingUser { get; } = submittingUser;
  public DateTime? TimeSubmitted { get; } = timeSubmitted;
  public string? Type { get; } = type;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
