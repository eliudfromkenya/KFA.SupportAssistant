using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.UserEndPoints;

public class ChangeRoleRequest
{
  public const string Route = "/users/change_role";

  [Required]
  public string? UserId { get; set; }

  [Required]
  public string? NewRoleId { get; set; }

  public string? Device { get; set; }
}
