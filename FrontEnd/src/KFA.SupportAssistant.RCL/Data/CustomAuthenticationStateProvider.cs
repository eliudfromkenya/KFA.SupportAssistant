using Blazored.LocalStorage;
using KFA.SupportAssistant.RCL.Models.Data;
using KFA.SupportAssistant.RCL.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KFA.SupportAssistant.RCL.Data;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
  public ILocalStorageService _localStorageService { get; }
  public IUserService _userService { get; set; }

  public CustomAuthenticationStateProvider(ILocalStorageService localStorageService,
      IUserService userService)
  {
    //throw new Exception("CustomAuthenticationStateProviderException");
    _localStorageService = localStorageService;
    _userService = userService;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    SystemUserDTO? user = null;
    DateTime? date = null;
    if (await _localStorageService.ContainKeyAsync("date")) {
      date = await _localStorageService.GetItemAsync<DateTime>("date");
      user = await _localStorageService.GetItemAsync<SystemUserDTO>("user");
    }

    if(date > DateTime.Now)
    {
      await _localStorageService.SetItemAsync("date", DateTime.Now);
    }

    ClaimsIdentity identity;

    if (user != null && date > DateTime.Now.AddHours(-12))
    //if (user != null && date > DateTime.Now.AddMinutes(-5))
    {
      identity = user != null ? GetClaimsIdentity(user) : new ClaimsIdentity();
    }
    else
    {
      identity = new ClaimsIdentity();
    }

    var claimsPrincipal = new ClaimsPrincipal(identity);

    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
  }

  public async Task MarkUserAsAuthenticated(SystemUserDTO user, string token)
  {
    await _localStorageService.SetItemAsync("accessToken", token);
    await _localStorageService.SetItemAsync("refreshToken", token);
    await _localStorageService.SetItemAsync("date", DateTime.Now);
    await _localStorageService.SetItemAsync("user", user);
    await _localStorageService.SetItemAsync("name", user?.Username);

    var identity = GetClaimsIdentity(user);

    var claimsPrincipal = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
  }

  public async Task MarkUserAsLoggedOut()
  {
    await _localStorageService.RemoveItemAsync("refreshToken");
    await _localStorageService.RemoveItemAsync("accessToken");
    await _localStorageService.RemoveItemAsync("user");
    await _localStorageService.RemoveItemAsync("date");

    var identity = new ClaimsIdentity();

    var user = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
  }

  private ClaimsIdentity GetClaimsIdentity(SystemUserDTO? user)
  {
    var claimsIdentity = new ClaimsIdentity();

    if (user?.Username != null)
    {
      claimsIdentity = new ClaimsIdentity(new[]
                      {
                                  new Claim(ClaimTypes.Name, user.Username),
                                  new Claim(ClaimTypes.Role, user.RoleId??"None"),
                                  new Claim("IsUserEmployedBefore1990", IsUserEmployedBefore1990(user))
                              }, "apiauth_type");
    }

    return claimsIdentity;
  }

  private string IsUserEmployedBefore1990(SystemUserDTO? user)
  {
    if (user?.ExpirationDate?.Year < 1990)
      return "true";
    else
      return "false";
  }
}
