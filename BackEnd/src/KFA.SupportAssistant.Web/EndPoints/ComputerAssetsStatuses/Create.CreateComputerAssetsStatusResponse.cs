namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

public readonly struct CreateComputerAssetsStatusResponse(string? assetDetailID, string? assetsStatusID, global::System.DateTime? date, string? description, string? doneBy, string? status, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDetailID { get; } = assetDetailID;
  public string? AssetsStatusID { get; } = assetsStatusID;
  public global::System.DateTime? Date { get; } = date;
  public string? Description { get; } = description;
  public string? DoneBy { get; } = doneBy;
  public string? Status { get; } = status;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
