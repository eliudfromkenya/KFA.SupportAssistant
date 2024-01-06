﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace KFA.SupportAssistant.RCL.Models;
public readonly struct GeneralMenuItem
{
  public MainMenuType MainMenuType { get; init; }
  public string Name { get; init; }
  public string? SVGIcon { get; init; }
  public RenderFragment IconFragment => CreateIcon;
  public string Text { get; init; }
  public string URI { get; init; }
  public bool? IsVisible { get; init; }
  public string Group { get; init; }
  private void CreateIcon(RenderTreeBuilder builder)
  {
    builder.AddMarkupContent(0, SVGIcon);
  }
}
