namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public readonly struct CreateComputerAssetsPostsToDynamicResponse(string? assetDetailID, global::System.DateTime? dateGenerated, string? description, string? postID, bool? posted, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDetailID { get; } = assetDetailID;
  public global::System.DateTime? DateGenerated { get; } = dateGenerated;
  public string? Description { get; } = description;
  public string? PostID { get; } = postID;
  public bool? Posted { get; } = posted;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
