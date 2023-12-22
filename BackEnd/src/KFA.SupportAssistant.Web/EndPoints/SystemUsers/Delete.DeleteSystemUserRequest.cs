namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

public record DeleteSystemUserRequest
{
  public const string Route = "/system_users/{userId}";
  public static string BuildRoute(string? userId) => Route.Replace("{userId}", userId);
  public string? UserId { get; set; }
}
