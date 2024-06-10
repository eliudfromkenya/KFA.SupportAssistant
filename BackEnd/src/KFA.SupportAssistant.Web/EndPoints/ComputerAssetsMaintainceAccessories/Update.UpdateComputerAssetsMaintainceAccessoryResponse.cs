namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

public class UpdateComputerAssetsMaintainceAccessoryResponse
{
  public UpdateComputerAssetsMaintainceAccessoryResponse(ComputerAssetsMaintainceAccessoryRecord computerAssetsMaintainceAccessory)
  {
    ComputerAssetsMaintainceAccessory = computerAssetsMaintainceAccessory;
  }

  public ComputerAssetsMaintainceAccessoryRecord ComputerAssetsMaintainceAccessory { get; set; }
}
