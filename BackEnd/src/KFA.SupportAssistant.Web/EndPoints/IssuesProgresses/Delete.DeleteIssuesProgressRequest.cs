namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

public record DeleteIssuesProgressRequest
{
  public const string Route = "/issues_progresses/{progressID}";
  public static string BuildRoute(string? progressID) => Route.Replace("{progressID}", progressID);
  public string? ProgressID { get; set; }
}
