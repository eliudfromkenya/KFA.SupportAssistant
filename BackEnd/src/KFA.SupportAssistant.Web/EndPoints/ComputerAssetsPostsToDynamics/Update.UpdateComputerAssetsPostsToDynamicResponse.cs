namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public class UpdateComputerAssetsPostsToDynamicResponse
{
  public UpdateComputerAssetsPostsToDynamicResponse(ComputerAssetsPostsToDynamicRecord computerAssetsPostsToDynamic)
  {
    ComputerAssetsPostsToDynamic = computerAssetsPostsToDynamic;
  }

  public ComputerAssetsPostsToDynamicRecord ComputerAssetsPostsToDynamic { get; set; }
}
