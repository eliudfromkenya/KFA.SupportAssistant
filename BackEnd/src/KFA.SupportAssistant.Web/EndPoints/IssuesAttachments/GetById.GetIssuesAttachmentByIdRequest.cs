namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

public class GetIssuesAttachmentByIdRequest
{
  public const string Route = "/issues_attachments/{attachmentID}";

  public static string BuildRoute(string? attachmentID) => Route.Replace("{attachmentID}", attachmentID);

  public string? AttachmentID { get; set; }
}
