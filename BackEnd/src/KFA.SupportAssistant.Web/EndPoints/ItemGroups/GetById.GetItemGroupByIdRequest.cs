namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

public class GetItemGroupByIdRequest
{
  public const string Route = "/item_groups/{groupId}";

  public static string BuildRoute(string? groupId) => Route.Replace("{groupId}", groupId);

  public string? GroupId { get; set; }
}
