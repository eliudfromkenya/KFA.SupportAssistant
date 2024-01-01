using Fluxor;

namespace KFA.SupportAssistant.RCL.State.MainTitle;

[FeatureState]
public class MainTitleState
{
  public string? MainTitle { get; }

  public MainTitleState(string mainTitle)
  {
    MainTitle = mainTitle;
  }

  public MainTitleState()
  {
  }
}
