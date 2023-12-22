namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

public record DeleteDataDeviceRequest
{
  public const string Route = "/data_devices/{deviceId}";
  public static string BuildRoute(string? deviceId) => Route.Replace("{deviceId}", deviceId);
  public string? DeviceId { get; set; }
}
