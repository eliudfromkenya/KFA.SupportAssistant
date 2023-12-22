namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

public class GetUserLoginByIdRequest
{
  public const string Route = "/user_logins/{loginId}";

  public static string BuildRoute(string? loginId) => Route.Replace("{loginId}", loginId);

  public string? LoginId { get; set; }
}
