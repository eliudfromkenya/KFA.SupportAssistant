using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.SupportAssistant.RCL.Models;
public readonly struct SideMenuItem
{
  public MainMenuType MainMenuType { get; init; }
  public string Name { get; init; }
  public string SVGIcon { get; init; }
  public string Text { get; init; }
  public string URI { get; init; }
  public bool? IsVisible { get; init; }
  public string Group { get; init; }
}
