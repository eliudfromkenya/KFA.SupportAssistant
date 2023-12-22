using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

public class PatchDefaultAccessRightRequest : JsonPatchDocument<DefaultAccessRightDTO>, IPlainTextRequest
{
  public const string Route = "/default_access_rights/{rightID}";

  public static string BuildRoute(string rightID) => Route.Replace("{rightID}", rightID);

  public string RightID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<DefaultAccessRightDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<DefaultAccessRightDTO>>(Content)!;
}
