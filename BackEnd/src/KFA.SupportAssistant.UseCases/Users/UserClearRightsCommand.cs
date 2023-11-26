using Ardalis.Result;

namespace KFA.SupportAssistant.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserClearRightsCommand(string userId, string[] userRightsIds, string? device) : Ardalis.SharedKernel.ICommand<Result<string[]>>;
