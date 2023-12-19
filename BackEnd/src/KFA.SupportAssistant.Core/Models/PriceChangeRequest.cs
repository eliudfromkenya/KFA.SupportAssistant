using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_price_change_requests")]
public sealed record class PriceChangeRequest : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_price_change_requests";
  [MaxLength(255, ErrorMessage = "Please attanded by must be 255 characters or less")]
  [Column("attanded_by")]
  public string? AttandedBy { get; init; }

  [MaxLength(255, ErrorMessage = "Please batch number must be 255 characters or less")]
  [Column("batch_number")]
  public string? BatchNumber { get; init; }

  [Required]
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }
  [NotMapped]
  public string? CostCentre_Caption { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please cost price must be 255 characters or less")]
  [Column("cost_price")]
  public string? CostPrice { get; init; }

  [Required]
  [Column("item_code")]
  public string? ItemCode { get; init; }

  [ForeignKey(nameof(ItemCode))]
  public StockItem? Item { get; set; }
  [NotMapped]
  public string? Item_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("request_id")]
  public override string? Id { get; set; }

  [MaxLength(255, ErrorMessage = "Please requesting user must be 255 characters or less")]
  [Column("requesting_user")]
  public string? RequestingUser { get; init; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please selling price must be 255 characters or less")]
  [Column("selling_price")]
  public string? SellingPrice { get; init; }

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public string? Status { get; init; }

  [MaxLength(255, ErrorMessage = "Please time attended must be 255 characters or less")]
  [Column("time_attended")]
  public string? TimeAttended { get; init; }

  [MaxLength(255, ErrorMessage = "Please time of request must be 255 characters or less")]
  [Column("time_of_request")]
  public DateTime? TimeOfRequest { get; init; }
  public override object ToBaseDTO()
  {
    return (PriceChangeRequestDTO)this;
  }
}
