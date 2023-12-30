using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KFA.SupportAssistant.RCL.Pages.Users.Login;

namespace KFA.SupportAssistant.RCL.State.MainTitle;
public readonly struct ChangeLoggedInUserAction
{
   public LoginResponse? User { get; init; }
}
