using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

public record UpdateVerificationRightRequest
{
  public const string Route = "/verification_rights/{verificationRightId}";
  public string? DeviceId { get; set; }
  public string? UserId { get; set; }
  [Required]
  public string? UserRoleId { get; set; }
  [Required]
  public string? VerificationRightId { get; set; }
  [Required]
  public string? VerificationTypeId { get; set; }
}
