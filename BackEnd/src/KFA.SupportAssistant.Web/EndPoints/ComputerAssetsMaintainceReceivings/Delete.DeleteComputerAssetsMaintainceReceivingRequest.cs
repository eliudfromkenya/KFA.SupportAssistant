namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

public record DeleteComputerAssetsMaintainceReceivingRequest
{
  public const string Route = "/computer_assets_maintaince_receivings/{receiveID}";
  public static string BuildRoute(string? receiveID) => Route.Replace("{receiveID}", receiveID);
  public string? ReceiveID { get; set; }
}
