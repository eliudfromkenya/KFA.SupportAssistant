using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

public record UpdateDefaultAccessRightRequest
{
  public const string Route = "/default_access_rights/{rightID}";
  [Required]
  public string? Name { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? RightID { get; set; }
  [Required]
  public string? Rights { get; set; }
  [Required]
  public string? Type { get; set; }
}
