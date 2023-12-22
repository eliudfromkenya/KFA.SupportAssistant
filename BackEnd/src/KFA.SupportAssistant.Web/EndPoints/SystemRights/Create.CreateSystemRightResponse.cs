namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

public readonly struct CreateSystemRightResponse(bool? isCompulsory, string? narration, string? rightId, string? rightName, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public bool? IsCompulsory { get; } = isCompulsory;
  public string? Narration { get; } = narration;
  public string? RightId { get; } = rightId;
  public string? RightName { get; } = rightName;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
