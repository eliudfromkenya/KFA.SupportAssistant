using System.ComponentModel.DataAnnotations;
using KFA.SupportAssistant.Core.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_vendors")]
public sealed record class Vendor : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_vendors";
  [MaxLength(255, ErrorMessage = "Please contact must be 255 characters or less")]
  [Column("contact")]
  public string? Contact { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please descriptions must be 255 characters or less")]
  [Column("descriptions")]
  public string? Descriptions { get; init; }

  [MaxLength(255, ErrorMessage = "Please email must be 255 characters or less")]
  [Column("email")]
  public string? Email { get; init; }

  [Required]
  [Column("is_active")]
  public bool IsActive { get; init; }

  [Required]
  [Column("vendor_code")]
  public override string? Id { get; init; }

  public ICollection<ComputerAssetAcquisition>? ComputerAssetAcquisitions { get; set; }
  public ICollection<ComputerAssetsMaintainceAccessory>? ComputerAssetsMaintainceAccessories { get; set; }

  public override object ToBaseDTO()
  {
    return (VendorDTO)this;
  }
}
