using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_suppliers")]
public sealed record class Supplier : BaseModel
{
  public override object ToBaseDTO()
  {
    return (SupplierDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_suppliers";
  [MaxLength(255, ErrorMessage = "Please address must be 255 characters or less")]
  [Column("address")]
  public string? Address { get; init; }

  [MaxLength(25, ErrorMessage = "Please contact person must be 25 characters or less")]
  [Column("contact_person")]
  public string? ContactPerson { get; init; }

  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  // [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(255, ErrorMessage = "Please email must be 255 characters or less")]
  [Column("email")]
  public string? Email { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please postal code must be 255 characters or less")]
  [Column("postal_code")]
  public string? PostalCode { get; init; }

  // [Required]
  [Column("supplier_code")]
  public string? SupplierCode { get; init; }

  [ForeignKey(nameof(SupplierCode))]
  public LedgerAccount? LedgerAccount { get; set; }
  [NotMapped]
  public string? Supplier_Caption { get; set; }

  // [Required]
  [Column("supplier_id")]
  public override string? Id { get; set; }

  [MaxLength(25, ErrorMessage = "Please telephone must be 25 characters or less")]
  [Column("telephone")]
  public string? Telephone { get; init; }

  [MaxLength(255, ErrorMessage = "Please town must be 255 characters or less")]
  [Column("town")]
  public string? Town { get; init; }
}
