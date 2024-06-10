namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

public class UpdateComputerAssetTransferResponse
{
  public UpdateComputerAssetTransferResponse(ComputerAssetTransferRecord computerAssetTransfer)
  {
    ComputerAssetTransfer = computerAssetTransfer;
  }

  public ComputerAssetTransferRecord ComputerAssetTransfer { get; set; }
}
