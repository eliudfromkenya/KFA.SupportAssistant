using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_system_users")]
public sealed record class SystemUser : BaseModel
{
  public override object ToBaseDTO()
  {
    return (SystemUser)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_system_users";
  //[Required]
  [MaxLength(255, ErrorMessage = "Please contact must be 255 characters or less")]
  [Column("contact")]
  public string? Contact { get; init; }

  //[Required]
  [MaxLength(255, ErrorMessage = "Please email address must be 255 characters or less")]
  [Column("email_address")]
  public string? EmailAddress { get; init; }

  //[Required]
  [Column("expiration_date")]
  public global::System.DateTime? ExpirationDate { get; init; }

  //[Required]
  [Column("is_active")]
  public bool? IsActive { get; init; }

  //[Required]
  [Column("maturity_date")]
  public global::System.DateTime? MaturityDate { get; init; }

  //[Required]
  [MaxLength(255, ErrorMessage = "Please name of the user must be 255 characters or less")]
  [Column("name_of_the_user")]
  public string? NameOfTheUser { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  //[Required]
  [Column("password_hash")]
  public byte[]? PasswordHash { get; init; }

  //[Required]
  [Column("password_salt")]
  public byte[]? PasswordSalt { get; init; }
  [Required]
  [Column("role_id")]
  public string? RoleId { get; init; }

  [ForeignKey(nameof(RoleId))]
  public UserRole? Role { get; set; }
  [NotMapped]
  public string? Role_Caption { get; set; }

  [Required]
  [Column("user_id")]
  public override string? Id { get; set; }

  //[Required]
  //[MaxLength(25, ErrorMessage = "Please user number must be 25 characters or less")]
  //[Column("user_number")]
  //public string? UserNumber { get; init; }

  //[Required]
  [MaxLength(255, ErrorMessage = "Please username must be 255 characters or less")]
  [Column("username")]
  public string? Username { get; init; }

  public ICollection<UserLogin>? UserLogins { get; set; }
  public ICollection<UserRight>? UserRights { get; set; }
  public ICollection<VerificationRight>? VerificationRights { get; set; }
}
