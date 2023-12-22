namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

public class GetDataDeviceByIdRequest
{
  public const string Route = "/data_devices/{deviceId}";

  public static string BuildRoute(string? deviceId) => Route.Replace("{deviceId}", deviceId);

  public string? DeviceId { get; set; }
}
