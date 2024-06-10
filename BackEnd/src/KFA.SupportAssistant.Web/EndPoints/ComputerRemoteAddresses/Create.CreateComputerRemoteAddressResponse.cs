namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

public readonly struct CreateComputerRemoteAddressResponse(string? anyDeskId, string? anyDeskNumber, string? anydeskPassword, string? assetDetailID, string? costCentreCode, string? deviceName, string? nameOfUser, string? narration, string? teamViewerAddress, string? type, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AnyDeskId { get; } = anyDeskId;
  public string? AnyDeskNumber { get; } = anyDeskNumber;
  public string? AnydeskPassword { get; } = anydeskPassword;
  public string? AssetDetailID { get; } = assetDetailID;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? DeviceName { get; } = deviceName;
  public string? NameOfUser { get; } = nameOfUser;
  public string? Narration { get; } = narration;
  public string? TeamViewerAddress { get; } = teamViewerAddress;
  public string? Type { get; } = type;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
