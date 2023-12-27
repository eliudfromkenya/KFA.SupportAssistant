using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.Web.UserEndPoints;

public readonly struct LoginResponse(string? loginId, string? token, object? userId, string? role, DateTime? date, string?[] rights, SystemUserDTO? user)
{
  public string? LoginId { get; } = loginId;
  public string? Token { get; } = token;
  public object? UserId { get; } = userId;
  public string? Role { get; } = role;
  public SystemUserDTO? User { get; } = user;
  public string?[] Rights { get; } = rights;
  public DateTime? Date { get; } = date;
}
