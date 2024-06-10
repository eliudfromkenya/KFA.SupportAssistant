namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

public record DeleteComputerAssetsMaintainceDispatchRequest
{
  public const string Route = "/computer_assets_maintaince_dispatches/{dispatchID}";
  public static string BuildRoute(string? dispatchID) => Route.Replace("{dispatchID}", dispatchID);
  public string? DispatchID { get; set; }
}
