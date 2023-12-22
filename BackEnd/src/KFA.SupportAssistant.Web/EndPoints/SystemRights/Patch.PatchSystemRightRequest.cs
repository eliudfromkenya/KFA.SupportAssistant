using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

public class PatchSystemRightRequest : JsonPatchDocument<SystemRightDTO>, IPlainTextRequest
{
  public const string Route = "/system_rights/{rightId}";

  public static string BuildRoute(string rightId) => Route.Replace("{rightId}", rightId);

  public string RightId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<SystemRightDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<SystemRightDTO>>(Content)!;
}
