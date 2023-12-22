using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

public class PatchVerificationRightRequest : JsonPatchDocument<VerificationRightDTO>, IPlainTextRequest
{
  public const string Route = "/verification_rights/{verificationRightId}";

  public static string BuildRoute(string verificationRightId) => Route.Replace("{verificationRightId}", verificationRightId);

  public string VerificationRightId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<VerificationRightDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<VerificationRightDTO>>(Content)!;
}
