namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

public class GetComputerAssetDetailByIdRequest
{
  public const string Route = "/computer_asset_details/{assetID}";

  public static string BuildRoute(string? assetID) => Route.Replace("{assetID}", assetID);

  public string? AssetID { get; set; }
}
