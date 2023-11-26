using Ardalis.Result;
using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.UseCases.Users;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserRegisterCommand(SystemUserDTO user, string? device, string password) : Ardalis.SharedKernel.ICommand<Result<(SystemUserDTO user, string? loginId, string?[]? rights)>>;
