using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

public class PatchUserAuditTrailRequest : JsonPatchDocument<UserAuditTrailDTO>, IPlainTextRequest
{
  public const string Route = "/user_audit_trails/{auditId}";

  public static string BuildRoute(string auditId) => Route.Replace("{auditId}", auditId);

  public string AuditId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<UserAuditTrailDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<UserAuditTrailDTO>>(Content)!;
}
