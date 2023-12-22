namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

public readonly struct CreatePriceChangeRequestResponse(string? attandedBy, string? batchNumber, string? costCentreCode, string? costPrice, string? itemCode, string? narration, string? requestID, string? requestingUser, string? sellingPrice, string? status, DateTime? timeAttended, DateTime? TimeOfRequest, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AttandedBy { get; } = attandedBy;
  public string? BatchNumber { get; } = batchNumber;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? CostPrice { get; } = costPrice;
  public string? ItemCode { get; } = itemCode;
  public string? Narration { get; } = narration;
  public string? RequestID { get; } = requestID;
  public string? RequestingUser { get; } = requestingUser;
  public string? SellingPrice { get; } = sellingPrice;
  public string? Status { get; } = status;
  public DateTime? TimeAttended { get; } = timeAttended;
  public DateTime? TimeOfRequest { get; } = TimeOfRequest;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
