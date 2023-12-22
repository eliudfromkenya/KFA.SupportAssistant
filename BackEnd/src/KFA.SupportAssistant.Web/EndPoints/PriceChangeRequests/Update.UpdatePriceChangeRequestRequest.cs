using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

public record UpdatePriceChangeRequestRequest
{
  public const string Route = "/price_change_requests/{requestID}";
  public string? AttandedBy { get; set; }
  public string? BatchNumber { get; set; }
  [Required]
  public string? CostCentreCode { get; set; }
  [Required]
  public string? CostPrice { get; set; }
  [Required]
  public string? ItemCode { get; set; }
  public string? Narration { get; set; }
  [Required]
  public string? RequestID { get; set; }
  public string? RequestingUser { get; set; }
  [Required]
  public string? SellingPrice { get; set; }
  public string? Status { get; set; }
  public string? TimeAttended { get; set; }
  public string? TimeOfRequest { get; set; }
}
