namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

public record DeleteUserRoleRequest
{
  public const string Route = "/user_roles/{roleId}";
  public static string BuildRoute(string? roleId) => Route.Replace("{roleId}", roleId);
  public string? RoleId { get; set; }
}
