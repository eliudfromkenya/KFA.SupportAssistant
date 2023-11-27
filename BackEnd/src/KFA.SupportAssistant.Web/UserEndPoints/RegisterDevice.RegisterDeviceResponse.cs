namespace KFA.SupportAssistant.Web.UserEndPoints;

public readonly struct RegisterDeviceResponse(string? typeOfDevice, string? stationID, string? deviceRight, string? deviceNumber, string? deviceName, string? deviceCode, string? deviceCaption, string id)
{
  public string? Description { get; init; } = deviceCaption;
  public string? DeviceCode { get; init; } = deviceCode;
  public string? DeviceName { get; init; } = deviceName;
  public string DeviceId { get; init; } = id;
  public string? DeviceNumber { get; init; } = deviceNumber;
  public string? DeviceRight { get; init; } = deviceRight;
  public string? StationID { get; init; } = stationID;
  public string? TypeOfDevice { get; init; } = typeOfDevice;
}
