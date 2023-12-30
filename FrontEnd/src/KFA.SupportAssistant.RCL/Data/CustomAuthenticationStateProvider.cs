﻿using Blazored.LocalStorage;
using Fluxor;
using KFA.SupportAssistant.RCL.Models.Data;
using KFA.SupportAssistant.RCL.Services;
using KFA.SupportAssistant.RCL.State.MainTitle;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using static KFA.SupportAssistant.RCL.Pages.Users.Login;

namespace KFA.SupportAssistant.RCL.Data;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
  readonly IDispatcher _dispatcher;
  readonly IState<LoggedInUserState> _userState;
  public ILocalStorageService _localStorageService { get; }
  public IUserService _userService { get; set; }

  public CustomAuthenticationStateProvider(ILocalStorageService localStorageService,
      IUserService userService, IDispatcher dispatcher, IState<LoggedInUserState> userState)
  {
    _dispatcher = dispatcher;
    _localStorageService = localStorageService;
    _userService = userService;
    _userState = userState;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    LoginResponse? user = _userState?.Value?.User;
    DateTime? date = DateTime.Now;

    if (user == null)
    {
      if (await _localStorageService.ContainKeyAsync("date"))
      {
        date = await _localStorageService.GetItemAsync<DateTime>("date");
        user = await _localStorageService.GetItemAsync<LoginResponse>("user");
        _dispatcher.Dispatch(new ChangeLoggedInUserAction { User = user });
      }

      if (date > DateTime.Now)
      {
        await _localStorageService.SetItemAsync("date", DateTime.Now);
      }
    }

    ClaimsIdentity identity;

    // if (user != null && date > DateTime.Now.AddHours(-12))
    if (user != null && date > DateTime.Now.AddMinutes(-10))
    {
      identity = user != null ? GetClaimsIdentity(user?.User) : new ClaimsIdentity();
    }
    else
    {
      identity = new ClaimsIdentity();
    }

    var claimsPrincipal = new ClaimsPrincipal(identity);

    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
  }

  public async Task MarkUserAsAuthenticated(LoginResponse? user)
  {
    await _localStorageService.SetItemAsync("date", DateTime.Now);
    await _localStorageService.SetItemAsync("user", user);
    await _localStorageService.SetItemAsync("name", user?.User?.Username ?? string.Empty);

    _dispatcher.Dispatch(new ChangeLoggedInUserAction { User = user });

    var identity = GetClaimsIdentity(user?.User);

    var claimsPrincipal = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
  }

  public async Task MarkUserAsLoggedOut()
  {
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
