namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

public class GetComputerAssetsMaintainceAccessoryByIdRequest
{
  public const string Route = "/computer_assets_maintaince_accessories/{accessoryID}";

  public static string BuildRoute(string? accessoryID) => Route.Replace("{accessoryID}", accessoryID);

  public string? AccessoryID { get; set; }
}
