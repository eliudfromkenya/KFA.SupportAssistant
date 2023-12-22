using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

public class CreateStockItemCodesRequestRequest
{
  public const string Route = "/stock_item_codes_requests";
  public string? AttandedBy { get; set; }

  [Required]
  public string? CostCentreCode { get; set; }

  [Required]
  public decimal? CostPrice { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? Distributor { get; set; }
  public string? ItemCode { get; set; }

  [Required]
  public string? ItemCodeRequestID { get; set; }

  public string? Narration { get; set; }
  public string? RequestingUser { get; set; }

  [Required]
  public decimal? SellingPrice { get; set; }

  public string? Status { get; set; }
  public string? Supplier { get; set; }
  public string? TimeAttended { get; set; }
  public string? TimeOfRequest { get; set; }
  public string? UnitOfMeasure { get; set; }
}
