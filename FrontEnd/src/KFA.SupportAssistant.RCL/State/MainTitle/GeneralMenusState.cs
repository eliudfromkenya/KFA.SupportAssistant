using Fluxor;
using KFA.SupportAssistant.RCL.Models;

namespace KFA.SupportAssistant.RCL.State.MainTitle;

[FeatureState]
public class GeneralMenusState
{
  public List<GeneralMenuItem> Menus { get; }
  public Exception? Error { get; }

  public GeneralMenusState(List<GeneralMenuItem>? menus, Exception? error)
  {
    Menus = menus ?? [];
    Error = error;
  }

  public GeneralMenusState()
  {
    Menus = [];
  }
}
