namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

public class GetComputerAssetAcquisitionByIdRequest
{
  public const string Route = "/computer_asset_acquisitions/{acquisitionID}";

  public static string BuildRoute(string? acquisitionID) => Route.Replace("{acquisitionID}", acquisitionID);

  public string? AcquisitionID { get; set; }
}
