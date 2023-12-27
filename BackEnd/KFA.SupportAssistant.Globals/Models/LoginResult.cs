namespace KFA.SupportAssistant.Globals.Models;

public readonly struct LoginResult
  {
  public string? LoginId { get; init; }
  public string? UserId { get; init; }
  public string? UserRole { get; init; }
  public object? User { get; init; }
  public string?[] UserRights { get; init; }
  public DateTime? ExpiryDate { get; init; }
  public string? Prefix { get; init; }
  public string? DeviceId { get; init; }
  }
