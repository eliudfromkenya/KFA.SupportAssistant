using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

public record UpdateDeviceGuidRequest
{
  public const string Route = "/device_guids/{guid}";
  [Required]
  public string? Guid { get; set; }
}
