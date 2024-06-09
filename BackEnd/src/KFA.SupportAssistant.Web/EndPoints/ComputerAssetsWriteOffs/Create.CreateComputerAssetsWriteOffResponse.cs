namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

public readonly struct CreateComputerAssetsWriteOffResponse(string? assetDetailID, string? description, string? narration, string? reasonForWriteOff, string? writeOffID, global::System.DateTime? writeOffDate, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDetailID { get; } = assetDetailID;
  public string? Description { get; } = description;
  public string? Narration { get; } = narration;
  public string? ReasonForWriteOff { get; } = reasonForWriteOff;
  public string? WriteOffID { get; } = writeOffID;
  public global::System.DateTime? WriteOffDate { get; } = writeOffDate;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
