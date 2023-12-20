using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SharedKernel;
using Autofac;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core;
public static class CoreFunctions
{
  public static string GetURL(string subURL) { return $"{subURL}".Replace("//", "/"); }

  static List<DefaultAccessRight>? _defaultAccessRights = null;
  public static async Task<string[]?> GetDefaultAccessRights(IRepository<DefaultAccessRight> repo, string name, string type)
   { 
      _defaultAccessRights ??= await repo.ListAsync();

    if (_defaultAccessRights == null)
      return null;

    return _defaultAccessRights
        .Where(c => c.Type == type && c.Name == name)
        .SelectMany(c => c.Rights?.Split(',') ?? [])
        .Select(c => c?.Trim()).Distinct()
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(c => c!)
        .ToArray();
   }

  public static async Task<string[]?> GetDefaultAccessRights(IRepository<DefaultAccessRight> repo, string rightId)
  {
    _defaultAccessRights ??= await repo.ListAsync();

    if (_defaultAccessRights == null)
      return null;

    return _defaultAccessRights
       .Where(c => c.Id == rightId)
       .SelectMany(c => c.Rights?.Split(',') ?? [])
       .Select(c => c?.Trim()).Distinct()
       .Where(x => !string.IsNullOrWhiteSpace(x))
       .Select(c => c!)
       .ToArray();
  }
}
