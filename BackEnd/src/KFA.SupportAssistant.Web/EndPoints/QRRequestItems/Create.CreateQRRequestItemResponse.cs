namespace KFA.SupportAssistant.Web.EndPoints.QRRequestItems;

public readonly struct CreateQRRequestItemResponse(string? cashSaleNumber, string? costCentreCode, string? hsCode, string? hsName, string? itemCode, string? itemName, string? narration, decimal? percentageDiscount, decimal? quantity, string? requestID, string? saleID, global::System.DateTime? time, decimal? totalAmount, string? unitOfMeasure, decimal? unitPrice, decimal? vATAmount, string? vATClass, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? CashSaleNumber { get; } = cashSaleNumber;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? HsCode { get; } = hsCode;
  public string? HsName { get; } = hsName;
  public string? ItemCode { get; } = itemCode;
  public string? ItemName { get; } = itemName;
  public string? Narration { get; } = narration;
  public decimal? PercentageDiscount { get; } = percentageDiscount;
  public decimal? Quantity { get; } = quantity;
  public string? RequestID { get; } = requestID;
  public string? SaleID { get; } = saleID;
  public global::System.DateTime? Time { get; } = time;
  public decimal? TotalAmount { get; } = totalAmount;
  public string? UnitOfMeasure { get; } = unitOfMeasure;
  public decimal? UnitPrice { get; } = unitPrice;
  public decimal? VATAmount { get; } = vATAmount;
  public string? VATClass { get; } = vATClass;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
