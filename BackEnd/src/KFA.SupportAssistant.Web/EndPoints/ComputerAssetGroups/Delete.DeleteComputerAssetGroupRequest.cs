namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

public record DeleteComputerAssetGroupRequest
{
  public const string Route = "/computer_asset_groups/{groupID}";
  public static string BuildRoute(string? groupID) => Route.Replace("{groupID}", groupID);
  public string? GroupID { get; set; }
}
