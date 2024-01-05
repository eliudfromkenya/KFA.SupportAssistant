using KFA.SupportAssistant.RCL.Models;

namespace KFA.SupportAssistant.RCL.State.MainTitle;

public readonly struct ChangeMainTitleAction
{
  public string MainTitle { get; init; }
}

public readonly struct ChangeMainMenuAction
{
  public SideMenuItem[] Menus { get; init; }
  public string? UserImageUrl { get; init; }
  public Exception? Error { get; init; } 
}

public readonly struct ChangeGeneralMenuAction
{
  public GeneralMenuItem[] Menus { get; init; }
  public Exception? Error { get; init; }
}
