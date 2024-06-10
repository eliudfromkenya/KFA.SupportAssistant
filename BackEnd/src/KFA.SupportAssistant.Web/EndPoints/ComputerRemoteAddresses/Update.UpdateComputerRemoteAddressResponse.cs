namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

public class UpdateComputerRemoteAddressResponse
{
  public UpdateComputerRemoteAddressResponse(ComputerRemoteAddressRecord computerRemoteAddress)
  {
    ComputerRemoteAddress = computerRemoteAddress;
  }

  public ComputerRemoteAddressRecord ComputerRemoteAddress { get; set; }
}
