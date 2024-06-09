namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

public class GetComputerAssetsStatusByIdRequest
{
  public const string Route = "/computer_assets_statuses/{assetsStatusID}";

  public static string BuildRoute(string? assetsStatusID) => Route.Replace("{assetsStatusID}", assetsStatusID);

  public string? AssetsStatusID { get; set; }
}
