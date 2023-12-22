using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

public record IssuesProgressRecord(string? Description, string? IssueID, string? Narration, string? ProgressID, string? ReportedBy, IssueStatus? Status, global::System.DateTime? Time, DateTime? DateInserted___, DateTime? DateUpdated___);
