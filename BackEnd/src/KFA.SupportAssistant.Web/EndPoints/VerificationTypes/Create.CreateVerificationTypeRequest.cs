using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

public class CreateVerificationTypeRequest
{
  public const string Route = "/verification_types";
  public string? Category { get; set; }
  public string? Narration { get; set; }

  [Required]
  public string? VerificationTypeId { get; set; }

  [Required]
  public string? VerificationTypeName { get; set; }
}
