using Fluxor;
using KFA.SupportAssistant.RCL.State.MainTitle;

namespace KFA.SupportAssistant.RCL.State.Users;

public static class Reducers
{
  [ReducerMethod]
  public static LoggedInUserState ReduceLoggedINUserAction(LoggedInUserState state, ChangeLoggedInUserAction titleAction) =>
        new(user: titleAction.User);
}
