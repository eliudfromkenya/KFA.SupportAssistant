using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

public record UpdateUserLoginRequest
{
  public const string Route = "/user_logins/{loginId}";
  [Required]
  public string? DeviceId { get; set; }
  [Required]
  public global::System.DateTime? FromDate { get; set; }
  [Required]
  public string? LoginId { get; set; }
  public string? Narration { get; set; }
  [Required]
  public global::System.DateTime? UptoDate { get; set; }
  [Required]
  public string? UserId { get; set; }
}
