namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

public class UpdateComputerAssetMaintainceResponse
{
  public UpdateComputerAssetMaintainceResponse(ComputerAssetMaintainceRecord computerAssetMaintaince)
  {
    ComputerAssetMaintaince = computerAssetMaintaince;
  }

  public ComputerAssetMaintainceRecord ComputerAssetMaintaince { get; set; }
}
