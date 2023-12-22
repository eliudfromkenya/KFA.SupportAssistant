namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

public record DeleteVerificationTypeRequest
{
  public const string Route = "/verification_types/{verificationTypeId}";
  public static string BuildRoute(string? verificationTypeId) => Route.Replace("{verificationTypeId}", verificationTypeId);
  public string? VerificationTypeId { get; set; }
}
