using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

public class CreateSystemRightRequest
{
  public const string Route = "/system_rights";

  [Required]
  public bool? IsCompulsory { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? RightId { get; set; }

  [Required]
  public string? RightName { get; set; }
}
