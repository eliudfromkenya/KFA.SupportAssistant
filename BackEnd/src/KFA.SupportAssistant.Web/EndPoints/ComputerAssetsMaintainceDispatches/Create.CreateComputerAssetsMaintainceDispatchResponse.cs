namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

public readonly struct CreateComputerAssetsMaintainceDispatchResponse(string? assetDetailID, string? collectedBy, global::System.DateTime? dateSend, string? description, string? dispatchID, string? narration, string? reasonToSend, string? sentBy, string? toDepartmentCode, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDetailID { get; } = assetDetailID;
  public string? CollectedBy { get; } = collectedBy;
  public global::System.DateTime? DateSend { get; } = dateSend;
  public string? Description { get; } = description;
  public string? DispatchID { get; } = dispatchID;
  public string? Narration { get; } = narration;
  public string? ReasonToSend { get; } = reasonToSend;
  public string? SentBy { get; } = sentBy;
  public string? ToDepartmentCode { get; } = toDepartmentCode;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
