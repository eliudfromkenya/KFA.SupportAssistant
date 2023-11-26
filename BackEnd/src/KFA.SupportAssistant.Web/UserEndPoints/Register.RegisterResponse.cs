namespace KFA.SupportAssistant.Web.UserEndPoints;
public readonly struct RegisterResponse(string? token, string? roleId, string? userId, string? contact, string? emailAddress, DateTime expirationDate, bool isActive, DateTime maturityDate, string? nameOfTheUser, string? narration, string? username)
{
  public string? Token { get; init; } = token;
  public string? RoleId { get; init; } = roleId;
  public string? UserId { get; init; } = userId;
  public string? Contact { get; init; } = contact;
  public string? EmailAddress { get; init; } = emailAddress;
  public DateTime ExpirationDate { get; init; } = expirationDate;
  public bool IsActive { get; init; } = isActive;
  public DateTime MaturityDate { get; init; } = maturityDate;
  public string? NameOfTheUser { get; init; } = nameOfTheUser;
  public string? Narration { get; init; } = narration;
  public string? Username { get; init; } = username;
}
