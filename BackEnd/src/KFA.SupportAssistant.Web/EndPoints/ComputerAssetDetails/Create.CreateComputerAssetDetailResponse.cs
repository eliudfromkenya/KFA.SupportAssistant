namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

public readonly struct CreateComputerAssetDetailResponse(string? assetID, string? assetName, string? description, string? groupID, string? serialNumber, string? state, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetID { get; } = assetID;
  public string? AssetName { get; } = assetName;
  public string? Description { get; } = description;
  public string? GroupID { get; } = groupID;
  public string? SerialNumber { get; } = serialNumber;
  public string? State { get; } = state;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
