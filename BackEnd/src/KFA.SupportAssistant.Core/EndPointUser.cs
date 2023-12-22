namespace KFA.SupportAssistant.Core;
public readonly record struct EndPointUser
{
  public string? UserId { get; init; }
  public string? LoginId { get; init; }
  public string? RoleId { get; init; }
  public string[]? Rights { get; init; }
}
