using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
