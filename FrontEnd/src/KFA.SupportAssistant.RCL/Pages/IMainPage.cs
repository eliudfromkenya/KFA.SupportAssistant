using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SupportAssistant.RCL.Models;

namespace KFA.SupportAssistant.RCL.Pages;
public interface IMainPage
{
  public GeneralMenuItem[]? MenuItems { get; set; }
}
