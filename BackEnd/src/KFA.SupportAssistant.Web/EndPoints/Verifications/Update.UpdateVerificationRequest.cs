using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

public record UpdateVerificationRequest
{
  public const string Route = "/verifications/{verificationId}";
  [Required]
  public global::System.DateTime? DateOfVerification { get; set; }
  [Required]
  public string? LoginId { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? RecordId { get; set; }
  [Required]
  public string? TableName { get; set; }
  [Required]
  public string? VerificationId { get; set; }
  [Required]
  public string? VerificationName { get; set; }
  public string? VerificationRecordId { get; set; }
  [Required]
  public string? VerificationTypeId { get; set; }
}
