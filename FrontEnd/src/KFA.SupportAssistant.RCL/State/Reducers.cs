using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluxor;
using KFA.SupportAssistant.RCL.State.MainTitle;

namespace KFA.SupportAssistant.RCL.State;
public static class Reducers
{
  [ReducerMethod]
  public static MainTitleState ReduceMainTitleAction(MainTitleState state, ChangeMainTitleAction titleAction) =>
        new(mainTitle: titleAction.MainTitle);

  [ReducerMethod]
  public static LoggedInUserState ReduceLoggedINUserAction(LoggedInUserState state, ChangeLoggedInUserAction titleAction) =>
        new(user: titleAction.User);
}
