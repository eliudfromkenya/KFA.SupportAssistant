using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

public class PatchCommandDetailRequest : JsonPatchDocument<CommandDetailDTO>, IPlainTextRequest
{
  public const string Route = "/command_details/{commandId}";

  public static string BuildRoute(string commandId) => Route.Replace("{commandId}", commandId);

  public string CommandId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<CommandDetailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<CommandDetailDTO>>(Content)!;
}
