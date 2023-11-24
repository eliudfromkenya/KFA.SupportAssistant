
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.Models;
using KFA.SupportAssistant.Infrastructure.Models;

namespace KFA.SupportAssistant.Core.Services;
public interface IAuthService
{
  Task<LoginResult?> LoginAsync(string username, string password, string? device, CancellationToken cancellationToken);
  Task<bool> AddUserRightsAsync(string userId, CancellationToken cancellationToken, params string[] rightsToAdd);
  Task<bool> ChangePasswordAsync(string userIdOrUsername, string oldPassword, string newPassword, CancellationToken cancellationToken);
  Task<bool> ChangeUserRoleAsync(string userId, string newRoleId, string oldRoleId, CancellationToken cancellationToken);
  Task<bool> ClearUserRightsAsync(string userId, CancellationToken cancellationToken, params string[] rightsToClear);
  Task<DataDevice> RegisterDeviceAsync(DataDevice dataDevice, CancellationToken cancellationToken);
  Task<SystemUser> RegisterUserAsync(SystemUser usr, string password, CancellationToken cancellationToken);
}
