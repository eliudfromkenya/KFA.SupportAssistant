using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

public record UpdateUserRoleRequest
{
  public const string Route = "/user_roles/{roleId}";
  [Required]
  public global::System.DateTime? ExpirationDate { get; set; }
  [Required]
  public global::System.DateTime? MaturityDate { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? RoleId { get; set; }
  [Required]
  public string? RoleName { get; set; }
}
