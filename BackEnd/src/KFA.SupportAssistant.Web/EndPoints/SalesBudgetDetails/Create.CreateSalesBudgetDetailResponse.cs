namespace KFA.SupportAssistant.Web.EndPoints.SalesBudgetDetails;

public readonly struct CreateSalesBudgetDetailResponse(string? batchKey, string? itemCode, byte? month, decimal? month01Quantity, decimal? month02Quantity, decimal? month03Quantity, decimal? month04Quantity, decimal? month05Quantity, decimal? month06Quantity, decimal? month07Quantity, decimal? month08Quantity, decimal? month09Quantity, decimal? month10Quantity, decimal? month11Quantity, decimal? month12Quantity, string? narration, string? salesBudgetDetailId, decimal? sellingPrice, decimal? unitCostPrice, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? BatchKey { get; } = batchKey;
  public string? ItemCode { get; } = itemCode;
  public byte? Month { get; } = month;
  public decimal? Month01Quantity { get; } = month01Quantity;
  public decimal? Month02Quantity { get; } = month02Quantity;
  public decimal? Month03Quantity { get; } = month03Quantity;
  public decimal? Month04Quantity { get; } = month04Quantity;
  public decimal? Month05Quantity { get; } = month05Quantity;
  public decimal? Month06Quantity { get; } = month06Quantity;
  public decimal? Month07Quantity { get; } = month07Quantity;
  public decimal? Month08Quantity { get; } = month08Quantity;
  public decimal? Month09Quantity { get; } = month09Quantity;
  public decimal? Month10Quantity { get; } = month10Quantity;
  public decimal? Month11Quantity { get; } = month11Quantity;
  public decimal? Month12Quantity { get; } = month12Quantity;
  public string? Narration { get; } = narration;
  public string? SalesBudgetDetailId { get; } = salesBudgetDetailId;
  public decimal? SellingPrice { get; } = sellingPrice;
  public decimal? UnitCostPrice { get; } = unitCostPrice;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
