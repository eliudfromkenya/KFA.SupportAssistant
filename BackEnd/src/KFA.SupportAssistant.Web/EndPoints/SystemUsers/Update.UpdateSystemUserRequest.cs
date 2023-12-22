using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

public record UpdateSystemUserRequest
{
  public const string Route = "/system_users/{userId}";
  [Required]
  public string? Contact { get; set; }
  [Required]
  public string? EmailAddress { get; set; }
  [Required]
  public global::System.DateTime? ExpirationDate { get; set; }
  [Required]
  public bool? IsActive { get; set; }
  [Required]
  public global::System.DateTime? MaturityDate { get; set; }
  [Required]
  public string? NameOfTheUser { get; set; }
  public string? Narration { get; set; }
  [Required]
  public byte[]? PasswordHash { get; set; }
  [Required]
  public byte[]? PasswordSalt { get; set; }
  [Required]
  public string? RoleId { get; set; }
  [Required]
  public string? UserId { get; set; }
  [Required]
  public string? Username { get; set; }
}
