namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

public record DeleteComputerAssetMaintainceRequest
{
  public const string Route = "/computer_asset_maintainces/{maintainceID}";
  public static string BuildRoute(string? maintainceID) => Route.Replace("{maintainceID}", maintainceID);
  public string? MaintainceID { get; set; }
}
