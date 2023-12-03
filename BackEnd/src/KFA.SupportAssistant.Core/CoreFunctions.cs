using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.SupportAssistant.Core;
public static class CoreFunctions
{
   public static string GetURL(string subURL) { return $"{subURL}".Replace("//", "/"); }
   
}
