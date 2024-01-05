using Fluxor;
using KFA.SupportAssistant.RCL.Models;

namespace KFA.SupportAssistant.RCL.State.MainTitle;

[FeatureState]
public class MainMenusState
{
  public List<SideMenuItem> Menus { get; }
  public string? UserImage { get; }
  public Exception? Error { get; }

  public MainMenusState(List<SideMenuItem>? menus, string? userImage, Exception? error)
  {
    Menus = menus ?? [];
    UserImage = userImage;
    Error = error;
  }
  
  public MainMenusState()
  {
    Menus = [];
  }
}
