using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals.DataLayer;

namespace KFA.SupportAssistant.Infrastructure.Services;
internal class EndPointManager(IRepository<DefaultAccessRight> repo)
: IEndPointManager
{
  public string[] GetDefaultAccessRights(string name, string type)
  {
    return AsyncUtil.RunSync(() => CoreFunctions.GetDefaultAccessRights(repo, name, type)) ?? [];
  }

  public string[] GetDefaultAccessRights(string rightId)
  {
    return AsyncUtil.RunSync(() => CoreFunctions.GetDefaultAccessRights(repo, rightId)) ?? [];
  }
}
