namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

public record DeleteDeviceGuidRequest
{
  public const string Route = "/device_guids/{guid}";
  public static string BuildRoute(string? guid) => Route.Replace("{guid}", guid);
  public string? Guid { get; set; }
}
