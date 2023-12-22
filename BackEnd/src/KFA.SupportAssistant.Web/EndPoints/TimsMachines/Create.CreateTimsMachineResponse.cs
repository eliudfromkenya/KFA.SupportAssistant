namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

public readonly struct CreateTimsMachineResponse(string? classType, byte? currentStatus, string? domainName, string? externalIPAddress, string? externalPortNumber, string? internalIPAddress, string? internalPortNumber, string? machineID, string? narration, bool? readyForUse, string? serialNumber, string? timsName, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? ClassType { get; } = classType;
  public byte? CurrentStatus { get; } = currentStatus;
  public string? DomainName { get; } = domainName;
  public string? ExternalIPAddress { get; } = externalIPAddress;
  public string? ExternalPortNumber { get; } = externalPortNumber;
  public string? InternalIPAddress { get; } = internalIPAddress;
  public string? InternalPortNumber { get; } = internalPortNumber;
  public string? MachineID { get; } = machineID;
  public string? Narration { get; } = narration;
  public bool? ReadyForUse { get; } = readyForUse;
  public string? SerialNumber { get; } = serialNumber;
  public string? TimsName { get; } = timsName;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
