namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

public class UpdateIssuesAttachmentResponse
{
  public UpdateIssuesAttachmentResponse(IssuesAttachmentRecord issuesAttachment)
  {
    IssuesAttachment = issuesAttachment;
  }

  public IssuesAttachmentRecord IssuesAttachment { get; set; }
}
