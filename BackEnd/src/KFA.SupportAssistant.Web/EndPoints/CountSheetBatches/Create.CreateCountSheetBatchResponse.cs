namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

public readonly struct CreateCountSheetBatchResponse(string? batchKey, string? batchNumber, string? classOfCard, short? computerNumberOfRecords, decimal? computerTotalAmount, string? costCentreCode, global::System.DateTime? date, string? month, string? narration, short? noOfRecords, decimal? totalAmount, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? BatchKey { get; } = batchKey;
  public string? BatchNumber { get; } = batchNumber;
  public string? ClassOfCard { get; } = classOfCard;
  public short? ComputerNumberOfRecords { get; } = computerNumberOfRecords;
  public decimal? ComputerTotalAmount { get; } = computerTotalAmount;
  public string? CostCentreCode { get; } = costCentreCode;
  public global::System.DateTime? Date { get; } = date;
  public string? Month { get; } = month;
  public string? Narration { get; } = narration;
  public short? NoOfRecords { get; } = noOfRecords;
  public decimal? TotalAmount { get; } = totalAmount;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
