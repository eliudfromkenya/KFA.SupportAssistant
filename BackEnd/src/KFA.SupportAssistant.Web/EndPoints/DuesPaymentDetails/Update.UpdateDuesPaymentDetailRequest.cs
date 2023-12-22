using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

public record UpdateDuesPaymentDetailRequest
{
  public const string Route = "/dues_payment_details/{paymentID}";
  [Required]
  public decimal? Amount { get; set; }
  [Required]
  public global::System.DateTime? Date { get; set; }
  public string? DocumentNo { get; set; }
  [Required]
  public string? EmployeeID { get; set; }
  [Required]
  public bool? IsFinalPayment { get; set; }
  public string? Narration { get; set; }
  [Required]
  public decimal? OpeningBalance { get; set; }
  [Required]
  public string? PaymentID { get; set; }
  [Required]
  public string? PaymentType { get; set; }
  public string? ProcessedBy { get; set; }
}
