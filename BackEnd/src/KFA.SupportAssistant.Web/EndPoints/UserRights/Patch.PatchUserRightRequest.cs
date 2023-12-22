using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

public class PatchUserRightRequest : JsonPatchDocument<UserRightDTO>, IPlainTextRequest
{
  public const string Route = "/user_rights/{userRightId}";

  public static string BuildRoute(string userRightId) => Route.Replace("{userRightId}", userRightId);

  public string UserRightId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<UserRightDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<UserRightDTO>>(Content)!;
}
