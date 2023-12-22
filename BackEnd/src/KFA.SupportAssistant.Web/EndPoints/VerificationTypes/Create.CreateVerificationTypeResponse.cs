namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

public readonly struct CreateVerificationTypeResponse(string? category, string? narration, string? verificationTypeId, string? verificationTypeName, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Category { get; } = category;
  public string? Narration { get; } = narration;
  public string? VerificationTypeId { get; } = verificationTypeId;
  public string? VerificationTypeName { get; } = verificationTypeName;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
