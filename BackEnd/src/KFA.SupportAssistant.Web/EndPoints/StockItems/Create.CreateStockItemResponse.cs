namespace KFA.SupportAssistant.Web.EndPoints.StockItems;

public readonly struct CreateStockItemResponse(string? barcode, string? groupId, string? itemCode, string? itemName, string? narration, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Barcode { get; } = barcode;
  public string? GroupId { get; } = groupId;
  public string? ItemCode { get; } = itemCode;
  public string? ItemName { get; } = itemName;
  public string? Narration { get; } = narration;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
