using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

public record IssuesSubmissionRecord(string? IssueID, string? Narration, IssueStatus? Status, string? SubmissionID, string? SubmittedTo, string? SubmittingUser, DateTime? TimeSubmitted, string? Type, DateTime? DateInserted___, DateTime? DateUpdated___);
