namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

public record DeleteDefaultAccessRightRequest
{
  public const string Route = "/default_access_rights/{rightID}";
  public static string BuildRoute(string? rightID) => Route.Replace("{rightID}", rightID);
  public string? RightID { get; set; }
}
