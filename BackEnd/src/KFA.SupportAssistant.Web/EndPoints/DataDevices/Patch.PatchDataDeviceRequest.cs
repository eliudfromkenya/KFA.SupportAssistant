using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

public class PatchDataDeviceRequest : JsonPatchDocument<DataDeviceDTO>, IPlainTextRequest
{
  public const string Route = "/data_devices/{deviceId}";

  public static string BuildRoute(string deviceId) => Route.Replace("{deviceId}", deviceId);

  public string DeviceId { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<DataDeviceDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<DataDeviceDTO>>(Content)!;
}
