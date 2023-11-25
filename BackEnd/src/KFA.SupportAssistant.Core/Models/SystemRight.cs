using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_system_rights")]
public sealed record class SystemRight : BaseModel
{
  public override object ToBaseDTO()
  {
    return (SystemRightDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_system_rights";
  [Required]
  [Column("is_compulsory")]
  public bool IsCompulsory { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("right_id")]
  public override string? Id { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please right name must be 255 characters or less")]
  [Column("right_name")]
  public string? RightName { get; init; }

  public ICollection<UserRight>? UserRights { get; set; }
}
