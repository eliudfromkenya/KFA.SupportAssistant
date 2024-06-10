namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

public record DeleteComputerAssetTransferRequest
{
  public const string Route = "/computer_asset_transfers/{transferID}";
  public static string BuildRoute(string? transferID) => Route.Replace("{transferID}", transferID);
  public string? TransferID { get; set; }
}
