namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

public class UpdateIssuesProgressResponse
{
  public UpdateIssuesProgressResponse(IssuesProgressRecord issuesProgress)
  {
    IssuesProgress = issuesProgress;
  }

  public IssuesProgressRecord IssuesProgress { get; set; }
}
