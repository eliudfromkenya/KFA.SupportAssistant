using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

public record UpdateVerificationTypeRequest
{
  public const string Route = "/verification_types/{verificationTypeId}";
  public string? Category { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? VerificationTypeId { get; set; }
  [Required]
  public string? VerificationTypeName { get; set; }
}
