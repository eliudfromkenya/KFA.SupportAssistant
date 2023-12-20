using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_cost_centres")]
public sealed record class CostCentre : BaseModel
{
  public override object ToBaseDTO()
  {
    return (CostCentreDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_cost_centres";
  // [Required]
  [Column("cost_centre_code")]
  public override string? Id { get; set; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Column("is_active")]
  public bool? IsActive { get; init; } = true;

  [MaxLength(255, ErrorMessage = "Please region must be 255 characters or less")]
  [Column("region")]
  public string? Region { get; init; }

  [MaxLength(10, ErrorMessage = "Please supplier code prefix must be 10 characters or less")]
  [Column("supplier_code_prefix")]
  public string? SupplierCodePrefix { get; init; }

  public ICollection<ComputerAnydesk>? ComputerAnydesks { get; set; }
  public ICollection<DataDevice>? DataDevices { get; set; }
  public ICollection<LeasedPropertiesAccount>? LeasedPropertiesAccounts { get; set; }
  public ICollection<LedgerAccount>? LedgerAccounts { get; set; }
  public ICollection<LetPropertiesAccount>? LetPropertiesAccounts { get; set; }
  public ICollection<QRCodesRequest>? QRCodesRequests { get; set; }
  public ICollection<QRRequestItem>? QRRequestItems { get; set; }
  public ICollection<Supplier>? Suppliers { get; set; }
}
