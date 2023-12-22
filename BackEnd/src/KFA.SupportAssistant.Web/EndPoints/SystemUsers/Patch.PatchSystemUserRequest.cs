using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

public class PatchSystemUserRequest : JsonPatchDocument<SystemUserDTO>, IPlainTextRequest
{
  public const string Route = "/system_users/{userId}";

  public static string BuildRoute(string userId) => Route.Replace("{userId}", userId);

  public string UserId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<SystemUserDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<SystemUserDTO>>(Content)!;
}
