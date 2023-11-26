namespace KFA.SupportAssistant.Web.UserEndPoints;

public readonly struct LoginResponse
{
  public LoginResponse(string? loginId, string? token, object? userId, string? role, DateTime? date, string?[] rights)
  {
    LoginId = loginId;
    Token = token;
    UserId = userId;
    Role = role;
    Rights = rights;
    Date = date;
  }

  public string? LoginId { get; }
  public string? Token { get; }
  public object? UserId { get; }
  public string? Role { get; }
  public string?[] Rights { get; }
  public DateTime? Date { get; }
}
