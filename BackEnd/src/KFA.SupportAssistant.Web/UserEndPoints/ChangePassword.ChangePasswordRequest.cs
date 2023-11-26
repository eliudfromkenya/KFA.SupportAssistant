using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.UserEndPoints;

public class ChangePasswordRequest
{
  public const string Route = "/users/change_password";

  [Required]
  public string? Username { get; set; }
  [Required]
  public string? CurrentPassword { get; set; }
  [Required]
  public string? NewPassword { get; set; }
  public string? Device { get; internal set; }
}
