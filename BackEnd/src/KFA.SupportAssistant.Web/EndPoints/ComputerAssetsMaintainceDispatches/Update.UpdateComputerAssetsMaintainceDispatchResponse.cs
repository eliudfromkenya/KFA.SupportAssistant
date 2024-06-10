namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

public class UpdateComputerAssetsMaintainceDispatchResponse
{
  public UpdateComputerAssetsMaintainceDispatchResponse(ComputerAssetsMaintainceDispatchRecord computerAssetsMaintainceDispatch)
  {
    ComputerAssetsMaintainceDispatch = computerAssetsMaintainceDispatch;
  }

  public ComputerAssetsMaintainceDispatchRecord ComputerAssetsMaintainceDispatch { get; set; }
}
