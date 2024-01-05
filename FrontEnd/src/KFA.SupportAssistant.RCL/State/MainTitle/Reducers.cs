using Fluxor;
using KFA.SupportAssistant.RCL.State.MainTitle;

namespace KFA.SupportAssistant.RCL.State;

public static class Reducers
{
  [ReducerMethod]
  public static MainTitleState ReduceMainTitleAction(MainTitleState state, ChangeMainTitleAction titleAction) =>
        new(mainTitle: titleAction.MainTitle);

  [ReducerMethod]
  public static GeneralMenusState ReducerGeneralMenuChangedAction(GeneralMenusState state, ChangeGeneralMenuAction titleAction) =>
        new(menus: titleAction.Menus?.ToList() ?? [], error: titleAction.Error);

  [ReducerMethod]
  public static MainMenusState ReducerMainMenuChangedAction(MainMenusState state, ChangeMainMenuAction titleAction) =>
      new(menus: titleAction.Menus?.ToList() ?? [], userImage: titleAction.UserImageUrl, error: titleAction.Error);
}
