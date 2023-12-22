namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

public class GetUserAuditTrailByIdRequest
{
  public const string Route = "/user_audit_trails/{auditId}";

  public static string BuildRoute(string? auditId) => Route.Replace("{auditId}", auditId);

  public string? AuditId { get; set; }
}
