using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.UserEndPoints;

public class AddRightsRequest
{
  public const string Route = "/users/add_rights";

  [Required]
  public string? UserId { get; set; }

  [Required]
  public string?[]? Rights { get; set; }

  [Required]
  public string?[]? Commands { get; set; }
}
