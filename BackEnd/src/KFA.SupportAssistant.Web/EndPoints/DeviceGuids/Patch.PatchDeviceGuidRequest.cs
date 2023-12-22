using KFA.SupportAssistant.Core.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

public class PatchDeviceGuidRequest : JsonPatchDocument<DeviceGuidDTO>, IPlainTextRequest
{
  public const string Route = "/device_guids/{guid}";

  public static string BuildRoute(string guid) => Route.Replace("{guid}", guid);

  public string Guid { get; set; } = string.Empty;
  public string Content { get; set; } = string.Empty;

  public JsonPatchDocument<DeviceGuidDTO> PatchDocument
      => Newtonsoft.Json.JsonConvert.DeserializeObject<JsonPatchDocument<DeviceGuidDTO>>(Content)!;
}
