namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

public class UpdateComputerAssetsMaintainceReceivingResponse
{
  public UpdateComputerAssetsMaintainceReceivingResponse(ComputerAssetsMaintainceReceivingRecord computerAssetsMaintainceReceiving)
  {
    ComputerAssetsMaintainceReceiving = computerAssetsMaintainceReceiving;
  }

  public ComputerAssetsMaintainceReceivingRecord ComputerAssetsMaintainceReceiving { get; set; }
}
