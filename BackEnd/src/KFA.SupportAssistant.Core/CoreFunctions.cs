using Ardalis.SharedKernel;
using Autofac;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core;

public static class CoreFunctions
{
  public static string GetURL(string subURL)
  { return $"{subURL}".Replace("//", "/"); }

  private static List<DefaultAccessRight>? _defaultAccessRights = null;
  private static readonly object _lockObj = new();

  public static string[]? GetDefaultAccessRights(IRepository<DefaultAccessRight> repo, string name, string type)
  {
    lock (_lockObj)
      if (_defaultAccessRights == null)
      {
        _defaultAccessRights = AsyncUtil.RunSync(() => repo.ListAsync());
        SetDestroyRights();
      }

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

  // This function will be called to destroy loaded access rights when application is done loading to free up space
  private static void SetDestroyRights()
  {
    Functions.RunOnBackground(() =>
    {
      try
      {
        Thread.Sleep(60000);
        _defaultAccessRights = null;
      }
      catch
      {
      }
    });
  }

  public static string[]? GetDefaultAccessRights(IRepository<DefaultAccessRight> repo, string rightId)
  {
    lock (_lockObj)
      if (_defaultAccessRights == null)
      {
        _defaultAccessRights = AsyncUtil.RunSync(() => repo.ListAsync());
        SetDestroyRights();
      }

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
