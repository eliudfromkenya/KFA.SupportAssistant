using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

public class PatchIssuesAttachmentRequest : JsonPatchDocument<IssuesAttachmentDTO>, IPlainTextRequest
{
  public const string Route = "/issues_attachments/{attachmentID}";

  public static string BuildRoute(string attachmentID) => Route.Replace("{attachmentID}", attachmentID);

  public string AttachmentID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<IssuesAttachmentDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<IssuesAttachmentDTO>>(Content)!;
}
