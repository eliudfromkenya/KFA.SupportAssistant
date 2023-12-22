namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

public readonly struct CreateUserLoginResponse(string? deviceId, global::System.DateTime? fromDate, string? loginId, string? narration, global::System.DateTime? uptoDate, string? userId, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? DeviceId { get; } = deviceId;
  public global::System.DateTime? FromDate { get; } = fromDate;
  public string? LoginId { get; } = loginId;
  public string? Narration { get; } = narration;
  public global::System.DateTime? UptoDate { get; } = uptoDate;
  public string? UserId { get; } = userId;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
