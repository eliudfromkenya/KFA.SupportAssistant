using Fluxor;
using static KFA.SupportAssistant.RCL.Pages.Users.Login;

namespace KFA.SupportAssistant.RCL.State.MainTitle;

[FeatureState]
public class LoggedInUserState
{
  public LoginResponse? User { get; }

  public LoggedInUserState(LoginResponse? user)
  {
    User = user;
  }

  public LoggedInUserState()
  {
  }
}
