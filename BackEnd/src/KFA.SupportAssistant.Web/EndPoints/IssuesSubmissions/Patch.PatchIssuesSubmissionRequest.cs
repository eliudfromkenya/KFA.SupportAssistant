using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

public class PatchIssuesSubmissionRequest : JsonPatchDocument<IssuesSubmissionDTO>, IPlainTextRequest
{
  public const string Route = "/issues_submissions/{submissionID}";

  public static string BuildRoute(string submissionID) => Route.Replace("{submissionID}", submissionID);

  public string SubmissionID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<IssuesSubmissionDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<IssuesSubmissionDTO>>(Content)!;
}
