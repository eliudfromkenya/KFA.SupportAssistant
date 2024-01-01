
namespace KFA.SupportAssistant.Core.DTOs;
public record class DuesPaymentDetailDTO : BaseDTO
{
  public decimal Amount { get; set; }
  public global::System.DateTime? Date { get; set; }
  public string? DocumentNo { get; set; }
  public string? EmployeeID { get; set; }
  public bool? IsFinalPayment { get; set; }
  public string? Narration { get; set; }
  public decimal? OpeningBalance { get; set; }
  public string? PaymentType { get; set; }
  public string? ProcessedBy { get; set; }
}
