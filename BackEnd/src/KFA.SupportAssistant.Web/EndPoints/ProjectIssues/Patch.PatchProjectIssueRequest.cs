using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

public class PatchProjectIssueRequest : JsonPatchDocument<ProjectIssueDTO>, IPlainTextRequest
{
  public const string Route = "/project_issues/{projectIssueID}";

  public static string BuildRoute(string projectIssueID) => Route.Replace("{projectIssueID}", projectIssueID);

  public string ProjectIssueID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<ProjectIssueDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<ProjectIssueDTO>>(Content)!;
}
