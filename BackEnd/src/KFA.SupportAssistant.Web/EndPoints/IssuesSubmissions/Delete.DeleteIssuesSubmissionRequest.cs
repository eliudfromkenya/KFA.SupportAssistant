namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

public record DeleteIssuesSubmissionRequest
{
  public const string Route = "/issues_submissions/{submissionID}";
  public static string BuildRoute(string? submissionID) => Route.Replace("{submissionID}", submissionID);
  public string? SubmissionID { get; set; }
}
