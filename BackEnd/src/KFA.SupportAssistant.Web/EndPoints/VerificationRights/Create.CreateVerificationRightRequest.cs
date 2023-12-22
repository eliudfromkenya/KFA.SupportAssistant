using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

public class CreateVerificationRightRequest
{
  public const string Route = "/verification_rights";
  public string? DeviceId { get; set; }
  public string? UserId { get; set; }

  [Required]
  public string? UserRoleId { get; set; }

  [Required]
  public string? VerificationRightId { get; set; }

  public string? VerificationTypeId { get; set; }
}
