namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

public readonly struct CreateActualBudgetVariancesBatchHeaderResponse(string? approvedBy, string? batchKey, string? batchNumber, decimal? cashSalesAmount, short? computerNumberOfRecords, decimal? computerTotalActualAmount, decimal? computerTotalBudgetAmount, string? costCentreCode, string? month, string? narration, short? numberOfRecords, string? preparedBy, decimal? purchasesesAmount, decimal? totalActualAmount, decimal? totalBudgetAmount, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? ApprovedBy { get; } = approvedBy;
  public string? BatchKey { get; } = batchKey;
  public string? BatchNumber { get; } = batchNumber;
  public decimal? CashSalesAmount { get; } = cashSalesAmount;
  public short? ComputerNumberOfRecords { get; } = computerNumberOfRecords;
  public decimal? ComputerTotalActualAmount { get; } = computerTotalActualAmount;
  public decimal? ComputerTotalBudgetAmount { get; } = computerTotalBudgetAmount;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Month { get; } = month;
  public string? Narration { get; } = narration;
  public short? NumberOfRecords { get; } = numberOfRecords;
  public string? PreparedBy { get; } = preparedBy;
  public decimal? PurchasesesAmount { get; } = purchasesesAmount;
  public decimal? TotalActualAmount { get; } = totalActualAmount;
  public decimal? TotalBudgetAmount { get; } = totalBudgetAmount;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
