using KFA.SupportAssistant.RCL.Data;
using KFA.SupportAssistant.RCL.Models;
using static KFA.SupportAssistant.RCL.Pages.LoginPages.Login;

namespace KFA.SupportAssistant.RCL.Services;

public interface IUserService
{
  public Task<SystemUserDTO?> LoginAsync(LoginDetails loginDetails);

  public Task<SystemUserDTO?> RegisterUserAsync(SignupSystemUserDTO user);

  //public Task<SystemUserDTO?> GetUserByAccessTokenAsync(string accessToken);

  //public Task<SystemUserDTO?> RefreshTokenAsync(RefreshRequest refreshRequest);
}
