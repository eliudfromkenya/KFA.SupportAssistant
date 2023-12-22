namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

public class GetVerificationRightByIdRequest
{
  public const string Route = "/verification_rights/{verificationRightId}";

  public static string BuildRoute(string? verificationRightId) => Route.Replace("{verificationRightId}", verificationRightId);

  public string? VerificationRightId { get; set; }
}
