using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

public class CreateUserAuditTrailRequest
{
  public const string Route = "/user_audit_trails";

  [Required]
  public global::System.DateTime? ActivityDate { get; set; }

  [Required]
  public long? ActivityEnumNumber { get; set; }

  [Required]
  public string? AuditId { get; set; }

  [Required]
  public string? Category { get; set; }

  [Required]
  public string? CommandId { get; set; }

  [Required]
  public string? Data { get; set; }

  [Required]
  public string? Description { get; set; }

  [Required]
  public string? LoginId { get; set; }

  public string? Narration { get; set; }

  [Required]
  public string? OldValues { get; set; }
}
