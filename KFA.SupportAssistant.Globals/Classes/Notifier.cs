using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.SupportAssistant.Globals.Classes;
public static class Notifier
{
  public static void NotifyError(Exception? ex, string? v)
  {
    throw new NotImplementedException();
  }

  public static void NotifyError(string? message, string? v, Exception? ex)
  {
    //throw new NotImplementedException();
  }

  public static void NotifyError(Exception? ex)
  {
    throw new NotImplementedException();
  }
}
