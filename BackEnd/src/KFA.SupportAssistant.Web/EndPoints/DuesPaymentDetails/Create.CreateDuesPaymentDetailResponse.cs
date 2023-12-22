namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

public readonly struct CreateDuesPaymentDetailResponse(decimal? amount, global::System.DateTime? date, string? documentNo, string? employeeID, bool? isFinalPayment, string? narration, decimal? openingBalance, string? paymentID, string? paymentType, string? processedBy, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public decimal? Amount { get; } = amount;
  public global::System.DateTime? Date { get; } = date;
  public string? DocumentNo { get; } = documentNo;
  public string? EmployeeID { get; } = employeeID;
  public bool? IsFinalPayment { get; } = isFinalPayment;
  public string? Narration { get; } = narration;
  public decimal? OpeningBalance { get; } = openingBalance;
  public string? PaymentID { get; } = paymentID;
  public string? PaymentType { get; } = paymentType;
  public string? ProcessedBy { get; } = processedBy;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
