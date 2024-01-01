using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SupportAssistant.Globals.DataLayer;

namespace KFA.SupportAssistant;
public static class Declarations
{
  public static string ApplicationDataPath { get; internal set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DynamicAssistantHelper");
  public static string BaseApiUri = @"http://localhost:57678/api/v3/";
}
