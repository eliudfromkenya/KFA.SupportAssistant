namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

public class UpdateComputerAssetAcquisitionResponse
{
  public UpdateComputerAssetAcquisitionResponse(ComputerAssetAcquisitionRecord computerAssetAcquisition)
  {
    ComputerAssetAcquisition = computerAssetAcquisition;
  }

  public ComputerAssetAcquisitionRecord ComputerAssetAcquisition { get; set; }
}
