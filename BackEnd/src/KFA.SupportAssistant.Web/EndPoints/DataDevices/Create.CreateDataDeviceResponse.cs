namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

public readonly struct CreateDataDeviceResponse(string? deviceCaption, string? deviceCode, string? deviceId, string? deviceName, string? deviceNumber, string? deviceRight, string? stationID, string? typeOfDevice, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? DeviceCaption { get; } = deviceCaption;
  public string? DeviceCode { get; } = deviceCode;
  public string? DeviceId { get; } = deviceId;
  public string? DeviceName { get; } = deviceName;
  public string? DeviceNumber { get; } = deviceNumber;
  public string? DeviceRight { get; } = deviceRight;
  public string? StationID { get; } = stationID;
  public string? TypeOfDevice { get; } = typeOfDevice;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
