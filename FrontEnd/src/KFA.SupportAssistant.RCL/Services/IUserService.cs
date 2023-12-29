using KFA.SupportAssistant.RCL.Models.Data;
using static KFA.SupportAssistant.RCL.Pages.Users.Login;

namespace KFA.SupportAssistant.RCL.Services;

public interface IUserService
{
  public Task<LoginResponse?> LoginAsync(LoginDetails loginDetails);

  public Task<SystemUserDTO?> RegisterUserAsync(SignupSystemUserDTO user);

  //public Task<SystemUserDTO?> GetUserByAccessTokenAsync(string accessToken);

  //public Task<SystemUserDTO?> RefreshTokenAsync(RefreshRequest refreshRequest);
}
