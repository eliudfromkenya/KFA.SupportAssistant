namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

public readonly struct CreateComputerAssetsMaintainceReceivingResponse(string? assetDetailID, string? broughtBy, global::System.DateTime? dateRecieved, string? description, string? fromDepartmentCode, string? narration, string? reasonToSend, string? receiveID, string? recievedBy, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDetailID { get; } = assetDetailID;
  public string? BroughtBy { get; } = broughtBy;
  public global::System.DateTime? DateRecieved { get; } = dateRecieved;
  public string? Description { get; } = description;
  public string? FromDepartmentCode { get; } = fromDepartmentCode;
  public string? Narration { get; } = narration;
  public string? ReasonToSend { get; } = reasonToSend;
  public string? ReceiveID { get; } = receiveID;
  public string? RecievedBy { get; } = recievedBy;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
