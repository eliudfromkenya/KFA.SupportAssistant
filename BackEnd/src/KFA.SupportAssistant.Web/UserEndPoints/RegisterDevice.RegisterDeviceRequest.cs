using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.UserEndPoints;

public class RegisterDeviceRequest
{
  public const string Route = "/users/register_device";

  public string? Description { get; set; }
  public string? DeviceCaption { get; set; }
  public string? DeviceName { get; set; }
  public string? DeviceCode { get; set; }
  public string? DeviceRight { get; set; } = "Guest Device";
  public string? StationID { get; set; } = "1100";
  public string? TypeOfDevice { get; set; } = "Desktop";
}
