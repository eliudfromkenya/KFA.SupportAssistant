namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

public record DeleteVerificationRequest
{
  public const string Route = "/verifications/{verificationId}";
  public static string BuildRoute(string? verificationId) => Route.Replace("{verificationId}", verificationId);
  public string? VerificationId { get; set; }
}
