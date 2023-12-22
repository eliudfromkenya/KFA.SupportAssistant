namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

public readonly struct CreateDeviceGuidResponse(string? guid, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Guid { get; } = guid;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
