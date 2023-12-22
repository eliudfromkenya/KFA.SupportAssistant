namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

public class GetSystemRightByIdRequest
{
  public const string Route = "/system_rights/{rightId}";

  public static string BuildRoute(string? rightId) => Route.Replace("{rightId}", rightId);

  public string? RightId { get; set; }
}
