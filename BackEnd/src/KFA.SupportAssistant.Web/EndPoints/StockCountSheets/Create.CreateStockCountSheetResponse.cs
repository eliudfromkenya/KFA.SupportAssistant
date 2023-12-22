namespace KFA.SupportAssistant.Web.EndPoints.StockCountSheets;

public readonly struct CreateStockCountSheetResponse(decimal? actual, decimal? averageAgeMonths, string? batchKey, long? countSheetDocumentId, string? countSheetId, string? documentNumber, string? itemCode, string? narration, decimal? quantityOnHand, decimal? quantitySoldLast12Months, decimal? sellingPrice, decimal? stocksOver, decimal? stocksShort, decimal? unitCostPrice, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public decimal? Actual { get; } = actual;
  public decimal? AverageAgeMonths { get; } = averageAgeMonths;
  public string? BatchKey { get; } = batchKey;
  public long? CountSheetDocumentId { get; } = countSheetDocumentId;
  public string? CountSheetId { get; } = countSheetId;
  public string? DocumentNumber { get; } = documentNumber;
  public string? ItemCode { get; } = itemCode;
  public string? Narration { get; } = narration;
  public decimal? QuantityOnHand { get; } = quantityOnHand;
  public decimal? QuantitySoldLast12Months { get; } = quantitySoldLast12Months;
  public decimal? SellingPrice { get; } = sellingPrice;
  public decimal? StocksOver { get; } = stocksOver;
  public decimal? StocksShort { get; } = stocksShort;
  public decimal? UnitCostPrice { get; } = unitCostPrice;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
