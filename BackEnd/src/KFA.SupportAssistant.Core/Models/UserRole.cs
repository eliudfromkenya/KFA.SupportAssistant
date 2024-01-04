using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_user_roles")]
public sealed record class UserRole : BaseModel
{
  public override object ToBaseDTO()
  {
    return (UserRoleDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_user_roles";
  // [Required]
  [Column("expiration_date")]
  public global::System.DateTime ExpirationDate { get; init; }

  // [Required]
  [Column("maturity_date")]
  public global::System.DateTime MaturityDate { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  // [Required]
  [Column("role_id")]
  public override string? Id { get; init; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please role name must be 255 characters or less")]
  [Column("role_name")]
  public string? RoleName { get; init; }

  //[Column("role_number")]
  //public short RoleNumber { get; init; }

  public ICollection<SystemUser>? SystemUsers { get; set; }
  public ICollection<UserRight>? UserRights { get; set; }
  public VerificationRight? VerificationRight { get; set; }
  [NotMapped]
  public string? UserRole_Caption { get; set; }
}
