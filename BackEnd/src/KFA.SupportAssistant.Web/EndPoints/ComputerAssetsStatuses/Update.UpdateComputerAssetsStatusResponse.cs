namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

public class UpdateComputerAssetsStatusResponse
{
  public UpdateComputerAssetsStatusResponse(ComputerAssetsStatusRecord computerAssetsStatus)
  {
    ComputerAssetsStatus = computerAssetsStatus;
  }

  public ComputerAssetsStatusRecord ComputerAssetsStatus { get; set; }
}
