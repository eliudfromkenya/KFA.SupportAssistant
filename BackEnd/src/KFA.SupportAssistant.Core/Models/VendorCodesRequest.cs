using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_vendor_codes_requests")]
public sealed record class VendorCodesRequest : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_vendor_codes_requests";
  [MaxLength(255, ErrorMessage = "Please attanded by must be 255 characters or less")]
  [Column("attanded_by")]
  public string? AttandedBy { get; init; }

  // [Required]
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

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [MaxLength(255, ErrorMessage = "Please requesting user must be 255 characters or less")]
  [Column("requesting_user")]
  public string? RequestingUser { get; init; }

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public string? Status { get; init; }

  [MaxLength(255, ErrorMessage = "Please time attended must be 255 characters or less")]
  [Column("time_attended")]
  public string? TimeAttended { get; init; }

  [MaxLength(255, ErrorMessage = "Please time of request must be 255 characters or less")]
  [Column("time_of_request")]
  public DateTime? TimeOfRequest { get; init; }

  [MaxLength(255, ErrorMessage = "Please vendor code must be 255 characters or less")]
  [Column("vendor_code")]
  public string? VendorCode { get; init; }

  // [Required]
  [Column("vendor_code_request_id")]
  public override string? Id { get; set; }

  [MaxLength(255, ErrorMessage = "Please vendor type must be 255 characters or less")]
  [Column("vendor_type")]
  public string? VendorType { get; init; }

  public override object ToBaseDTO()
  {
    return (VendorCodesRequestDTO)this;
  }
}
