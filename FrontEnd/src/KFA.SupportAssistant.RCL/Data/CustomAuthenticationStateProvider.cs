using System.Security.Claims;
using Blazored.LocalStorage;
using KFA.SupportAssistant.RCL.Models;
using KFA.SupportAssistant.RCL.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace KFA.SupportAssistant.RCL.Data;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
  public ILocalStorageService _localStorageService { get; }
  public IUserService _userService { get; set; }
  private readonly HttpClient _httpClient;

  public CustomAuthenticationStateProvider(ILocalStorageService localStorageService,
      IUserService userService,
      HttpClient httpClient)
  {
    //throw new Exception("CustomAuthenticationStateProviderException");
    _localStorageService = localStorageService;
    _userService = userService;
    _httpClient = httpClient;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");

    ClaimsIdentity identity;

    if (false && accessToken != null && accessToken != string.Empty)
    {
      //SystemUserDTO? user = await _userService.GetUserByAccessTokenAsync(accessToken??" ");
      //identity = user != null ? GetClaimsIdentity(user) : new ClaimsIdentity();
    }
    else
    {
      identity = new ClaimsIdentity();
    }

    var claimsPrincipal = new ClaimsPrincipal(identity);

    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
  }

  public async Task MarkUserAsAuthenticated(SystemUserDTO user)
  {
    await _localStorageService.SetItemAsync("accessToken", user.AccessToken);
    await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);

    var identity = GetClaimsIdentity(user);

    var claimsPrincipal = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
  }

  public async Task MarkUserAsLoggedOut()
  {
    await _localStorageService.RemoveItemAsync("refreshToken");
    await _localStorageService.RemoveItemAsync("accessToken");

    var identity = new ClaimsIdentity();

    var user = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
  }

  private ClaimsIdentity GetClaimsIdentity(SystemUserDTO? user)
  {
    var claimsIdentity = new ClaimsIdentity();

    if (user?.EmailAddress != null)
    {
      claimsIdentity = new ClaimsIdentity(new[]
                      {
                                  new Claim(ClaimTypes.Name, user.EmailAddress),
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
