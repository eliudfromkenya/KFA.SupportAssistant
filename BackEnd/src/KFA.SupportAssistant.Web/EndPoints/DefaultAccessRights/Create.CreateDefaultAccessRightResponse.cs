namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

public readonly struct CreateDefaultAccessRightResponse(string? name, string? narration, string? rightID, string? rights, string? type, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Name { get; } = name;
  public string? Narration { get; } = narration;
  public string? RightID { get; } = rightID;
  public string? Rights { get; } = rights;
  public string? Type { get; } = type;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
