namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetBatchHeaders;

public readonly struct CreateSalesBudgetBatchHeaderResponse(string? approvedBy, string? batchKey, string? batchNumber, short? computerNumberOfRecords, decimal? computerTotalAmount, string? costCentreCode, global::System.DateTime? date, string? monthFrom, string? monthTo, string? narration, short? numberOfRecords, string? preparedBy, decimal? totalAmount, decimal? totalQuantity, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? ApprovedBy { get; } = approvedBy;
  public string? BatchKey { get; } = batchKey;
  public string? BatchNumber { get; } = batchNumber;
  public short? ComputerNumberOfRecords { get; } = computerNumberOfRecords;
  public decimal? ComputerTotalAmount { get; } = computerTotalAmount;
  public string? CostCentreCode { get; } = costCentreCode;
  public global::System.DateTime? Date { get; } = date;
  public string? MonthFrom { get; } = monthFrom;
  public string? MonthTo { get; } = monthTo;
  public string? Narration { get; } = narration;
  public short? NumberOfRecords { get; } = numberOfRecords;
  public string? PreparedBy { get; } = preparedBy;
  public decimal? TotalAmount { get; } = totalAmount;
  public decimal? TotalQuantity { get; } = totalQuantity;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
