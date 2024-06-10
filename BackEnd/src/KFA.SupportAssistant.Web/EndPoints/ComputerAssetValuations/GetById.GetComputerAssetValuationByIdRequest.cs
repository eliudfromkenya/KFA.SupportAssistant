namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

public class GetComputerAssetValuationByIdRequest
{
  public const string Route = "/computer_asset_valuations/{revaluationID}";

  public static string BuildRoute(string? revaluationID) => Route.Replace("{revaluationID}", revaluationID);

  public string? RevaluationID { get; set; }
}
