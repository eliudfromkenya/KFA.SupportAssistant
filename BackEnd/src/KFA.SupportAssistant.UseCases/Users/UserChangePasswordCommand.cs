using Ardalis.Result;

namespace KFA.SupportAssistant.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserChangePasswordCommand(string userId, string currentPassword, string newPassword, string? device) : Ardalis.SharedKernel.ICommand<Result>;
