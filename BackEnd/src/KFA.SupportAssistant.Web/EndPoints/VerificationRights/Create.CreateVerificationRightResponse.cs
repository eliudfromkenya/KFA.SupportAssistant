namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

public readonly struct CreateVerificationRightResponse(string? deviceId, string? userId, string? userRoleId, string? verificationRightId, long? verificationTypeId, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? DeviceId { get; } = deviceId;
  public string? UserId { get; } = userId;
  public string? UserRoleId { get; } = userRoleId;
  public string? VerificationRightId { get; } = verificationRightId;
  public long? VerificationTypeId { get; } = verificationTypeId;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
