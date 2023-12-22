using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

public class PatchCommunicationMessageRequest : JsonPatchDocument<CommunicationMessageDTO>, IPlainTextRequest
{
  public const string Route = "/communication_messages/{messageID}";

  public static string BuildRoute(string messageID) => Route.Replace("{messageID}", messageID);

  public string MessageID { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<CommunicationMessageDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<CommunicationMessageDTO>>(Content)!;
}
