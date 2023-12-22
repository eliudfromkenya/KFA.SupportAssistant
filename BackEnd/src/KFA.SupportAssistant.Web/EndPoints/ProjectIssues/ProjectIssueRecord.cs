using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

public record ProjectIssueRecord(string? Category, global::System.DateTime? Date, string? Description, string? Effect, string? Narration, string? ProjectIssueID, string? RegisteredBy, IssueStatus? Status, string? SubCategory, string? Title, DateTime? DateInserted___, DateTime? DateUpdated___);
