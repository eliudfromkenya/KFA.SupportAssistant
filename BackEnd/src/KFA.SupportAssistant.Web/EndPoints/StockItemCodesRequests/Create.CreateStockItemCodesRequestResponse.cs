namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

public readonly struct CreateStockItemCodesRequestResponse(string? attandedBy, string? costCentreCode, decimal? costPrice, string? description, string? distributor, string? itemCode, string? itemCodeRequestID, string? narration, string? requestingUser, decimal? sellingPrice, string? status, string? supplier, DateTime? timeAttended, DateTime? TimeOfRequest, string? unitOfMeasure, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AttandedBy { get; } = attandedBy;
  public string? CostCentreCode { get; } = costCentreCode;
  public decimal? CostPrice { get; } = costPrice;
  public string? Description { get; } = description;
  public string? Distributor { get; } = distributor;
  public string? ItemCode { get; } = itemCode;
  public string? ItemCodeRequestID { get; } = itemCodeRequestID;
  public string? Narration { get; } = narration;
  public string? RequestingUser { get; } = requestingUser;
  public decimal? SellingPrice { get; } = sellingPrice;
  public string? Status { get; } = status;
  public string? Supplier { get; } = supplier;
  public DateTime? TimeAttended { get; } = timeAttended;
  public DateTime? TimeOfRequest { get; } = TimeOfRequest;
  public string? UnitOfMeasure { get; } = unitOfMeasure;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
