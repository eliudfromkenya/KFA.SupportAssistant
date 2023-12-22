namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

public readonly struct CreateVerificationResponse(global::System.DateTime? dateOfVerification, string? loginId, string? narration, long? recordId, string? tableName, string? verificationId, string? verificationName, long? verificationRecordId, long? verificationTypeId, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public global::System.DateTime? DateOfVerification { get; } = dateOfVerification;
  public string? LoginId { get; } = loginId;
  public string? Narration { get; } = narration;
  public long? RecordId { get; } = recordId;
  public string? TableName { get; } = tableName;
  public string? VerificationId { get; } = verificationId;
  public string? VerificationName { get; } = verificationName;
  public long? VerificationRecordId { get; } = verificationRecordId;
  public long? VerificationTypeId { get; } = verificationTypeId;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
