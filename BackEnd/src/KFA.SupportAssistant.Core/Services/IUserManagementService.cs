using Ardalis.Result;
using KFA.SupportAssistant.Core.DTOs;

namespace KFA.SupportAssistant.Core.Services;

public interface IUserManagementService
{
  Task<Result<UserRightDTO[]>> AddUserRightsAsync(string userId, string[] rightIds, string[] commandIds, CancellationToken cancellationToken);

  Task<Result> ChangeUserRoleAsync(string userId, string newRoleId, string? device, CancellationToken cancellationToken);

  Task<Result<string[]>> ClearUserRightsAsync(string userId, string? device, CancellationToken cancellationToken, params string[] rightsToClear);

}
