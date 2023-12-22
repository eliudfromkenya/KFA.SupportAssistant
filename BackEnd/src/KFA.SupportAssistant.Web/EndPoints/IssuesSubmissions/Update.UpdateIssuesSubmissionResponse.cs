namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

public class UpdateIssuesSubmissionResponse
{
  public UpdateIssuesSubmissionResponse(IssuesSubmissionRecord issuesSubmission)
  {
    IssuesSubmission = issuesSubmission;
  }

  public IssuesSubmissionRecord IssuesSubmission { get; set; }
}
