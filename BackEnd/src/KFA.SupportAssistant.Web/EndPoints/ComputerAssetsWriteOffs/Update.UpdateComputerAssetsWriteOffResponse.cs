namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

public class UpdateComputerAssetsWriteOffResponse
{
  public UpdateComputerAssetsWriteOffResponse(ComputerAssetsWriteOffRecord computerAssetsWriteOff)
  {
    ComputerAssetsWriteOff = computerAssetsWriteOff;
  }

  public ComputerAssetsWriteOffRecord ComputerAssetsWriteOff { get; set; }
}
