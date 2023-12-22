using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class DuesPaymentDetailDTO : BaseDTO<DuesPaymentDetail>
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
  public override DuesPaymentDetail? ToModel()
  {
    return (DuesPaymentDetail)this;
  }

  public static implicit operator DuesPaymentDetailDTO(DuesPaymentDetail obj)
  {
    return new DuesPaymentDetailDTO
    {
      Amount = decimal.TryParse(obj.Amount, out decimal amt) ? amt : 0,
      Date = DateTime.TryParse(obj.Date, out DateTime date) ? date : DateTime.MinValue,
      DocumentNo = obj.DocumentNo,
      EmployeeID = obj.EmployeeId,
      IsFinalPayment = bool.TryParse(obj.IsFinalPayment, out bool isFinal) ? isFinal : null,
      Narration = obj.Narration,
      OpeningBalance = decimal.TryParse(obj.OpeningBalance, out decimal bal) ? bal : 0,
      PaymentType = obj.PaymentType,
      ProcessedBy = obj.ProcessedBy,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator DuesPaymentDetail(DuesPaymentDetailDTO obj)
  {
    return new DuesPaymentDetail
    {
      Amount = obj.Amount.ToString(),
      Date = obj.Date?.ToString(),
      DocumentNo = obj.DocumentNo,
      EmployeeId = obj.EmployeeID,
      IsFinalPayment = obj.IsFinalPayment?.ToString(),
      Narration = obj.Narration,
      OpeningBalance = obj.OpeningBalance?.ToString(),
      PaymentType = obj.PaymentType,
      ProcessedBy = obj.ProcessedBy,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
