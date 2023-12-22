namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

public readonly struct CreateSystemUserResponse(string? contact, string? emailAddress, global::System.DateTime? expirationDate, bool? isActive, global::System.DateTime? maturityDate, string? nameOfTheUser, string? narration, string? roleId, string? userId, string? username, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? Contact { get; } = contact;
  public string? EmailAddress { get; } = emailAddress;
  public global::System.DateTime? ExpirationDate { get; } = expirationDate;
  public bool? IsActive { get; } = isActive;
  public global::System.DateTime? MaturityDate { get; } = maturityDate;
  public string? NameOfTheUser { get; } = nameOfTheUser;
  public string? Narration { get; } = narration;
  public string? RoleId { get; } = roleId;
  public string? UserId { get; } = userId;
  public string? Username { get; } = username;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
