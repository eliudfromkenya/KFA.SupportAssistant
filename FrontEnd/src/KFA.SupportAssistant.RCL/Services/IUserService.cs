using KFA.SupportAssistant.RCL.Data;

namespace KFA.SupportAssistant.RCL.Services;

public interface IUserService
{
  public Task<User?> LoginAsync(User user);

  public Task<User?> RegisterUserAsync(User user);

  public Task<User?> GetUserByAccessTokenAsync(string accessToken);

  public Task<User?> RefreshTokenAsync(RefreshRequest refreshRequest);
}
