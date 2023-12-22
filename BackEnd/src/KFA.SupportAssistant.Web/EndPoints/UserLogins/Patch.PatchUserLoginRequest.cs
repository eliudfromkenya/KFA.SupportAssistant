using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

public class PatchUserLoginRequest : JsonPatchDocument<UserLoginDTO>, IPlainTextRequest
{
  public const string Route = "/user_logins/{loginId}";

  public static string BuildRoute(string loginId) => Route.Replace("{loginId}", loginId);

  public string LoginId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<UserLoginDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<UserLoginDTO>>(Content)!;
}
