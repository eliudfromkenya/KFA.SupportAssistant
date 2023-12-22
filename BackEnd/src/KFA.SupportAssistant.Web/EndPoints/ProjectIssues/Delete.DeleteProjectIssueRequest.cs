namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

public record DeleteProjectIssueRequest
{
  public const string Route = "/project_issues/{projectIssueID}";
  public static string BuildRoute(string? projectIssueID) => Route.Replace("{projectIssueID}", projectIssueID);
  public string? ProjectIssueID { get; set; }
}
