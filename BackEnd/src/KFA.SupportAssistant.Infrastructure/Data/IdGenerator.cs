using System.Text.RegularExpressions;
using Humanizer;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Globals.Classes;
using KFA.SupportAssistant.Globals.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace KFA.SupportAssistant.Infrastructure.Data;

internal class IdGenerator : IIdGenerator
{
  public static string? prefix { get; private set; }

  private static Dictionary<string, string> IdNames { get; } = new();
  private static readonly Dictionary<string, string> PrimaryKeysSqls = new();
  private static readonly Dictionary<string, string> LastAssignedPrimary = new();
  private static readonly SortedDictionary<string, string> idsReg = new();
  private static bool HasRefreshedKeys = false;

  public async Task<bool?> RefreshKeysAsync(bool forceRefresh = false)
  {
    try
    {
      lock (LastAssignedPrimary)
      {
        if (HasRefreshedKeys && !forceRefresh)
          return null;
        if (Declarations.GeneralService == null)
          return null;
      }

      HasRefreshedKeys = true;
      List<TableMetaData?>? existings = null;
      try
      {
        existings = LocalCache.Get<TableMetaData>()?.ToList();
      }
      catch { throw; }
      if (existings == null) existings = new List<TableMetaData?>();

      try
      {
        prefix = Declarations.DeviceNumber;
        if (!string.IsNullOrWhiteSpace(prefix))
          LocalCache.Upsert("LastUsedPrefix", prefix);
      }
      catch { }

      var objs = await Declarations.GeneralService
          .RefreshKeys(Declarations.DeviceNumber).ConfigureAwait(false);

      foreach (var (obj, name) in from obj in objs
                                  let name = obj?.TableName?
                                  .StrimLineTableName()?.Pluralize()?.ToLower()?.MakeName()
                                  select (obj, name))
      {
        if (IdNames.ContainsKey(name))
          IdNames[name] = obj.TableName ?? "";
        else
          IdNames.Add(name, obj.TableName ?? "");

        if ((obj.LastAssignedValue ?? "").Length < 1)
          continue;

        var olds = existings.Where(c => c?.TableName == obj.TableName && (c?.LastAssignedValue?.StartsWith(prefix ?? "") ?? false));
        if (!olds.Any() ||
            (new[] { olds?.First()?.LastAssignedValue, obj.LastAssignedValue }.Max() == obj.LastAssignedValue))
          LocalCache.Upsert(name, obj);
      }
      LocalCache.Upsert("DeviceNumber", Declarations.DeviceNumber);
      return true;
    }
    catch (Exception ex)
    {
      Notifier.NotifyError($"Refreshing keys Error\r\n{ex.Message}", "Refreshing keys Error", ex);
      return false;
    }
  }

  public string GetNextId<T>() where T : BaseModel => GetNextId(typeof(T));

  public string GetNextId(Type type)
  {
    var tt = type?.Name?.GetModelName();
    if (!type?.IsAssignableTo(typeof(BaseModel)) ?? false)
      throw new InvalidCastException("Only models inheriting from 'IBaseModel' are allowed in the database context");

    if ((type?.IsInterface) ?? false && tt.StartsWith("I"))
    {
      tt = tt?[1..];
      if (tt?.EndsWith("Model") ?? false)
        tt = tt[0..^5];
    }
    return GetNextId(tt);
  }

  public bool SaveNewId<T>(string? id) => SaveNewId(id, typeof(T)?.Name?.GetModelName());

  public string GetNextId(string? table)
  {
    if (string.IsNullOrWhiteSpace(table))
      throw new NullReferenceException("Unable to get table name");

    if (string.IsNullOrEmpty(prefix))
    {
      try
      {
        prefix = Declarations.DeviceNumber;
        if (string.IsNullOrEmpty(prefix))
          prefix = LocalCache.Get<string>("LastUsedPrefix");
      }
      catch { }
    }

    var key = TryNextId(table);
    if (!string.IsNullOrEmpty(prefix) && (!key?.StartsWith(prefix) ?? false))
      key = prefix + "-01";

    if (!string.IsNullOrEmpty(key))
    {
      if (idsReg.ContainsKey(table))
      {
        if (idsReg[table] == key)
          key = GetNewId(key);
        idsReg[table] = key;
      }
      else
        idsReg.Add(table, key);
    }

    SaveNewId(key, table);
    return key ?? "";
  }

  public object RevertBack(string id)
  {
    var nos = id.GetNumbers();
    if (nos.Any())
    {
      var last = nos.Last();
      var nn = last.ToString();
      var index = id.LastIndexOf(nn);
      var index2 = id.LastIndexOf(nn) + nn.Length;
      var ans = $"{id[..index]}{last - 1}";
      if (index2 < id?.Length)
        ans = $"{ans}{id?[index2..]}";
      return ans;
    }
    else return id;
  }

  private bool SaveNewId(string? key, string? table)
  {
    try
    {
      table = table?.StrimLineTableName()?.Pluralize().MakeName().ToLower();
      if (LastAssignedPrimary.ContainsKey(table ?? ""))
        LastAssignedPrimary[table ?? ""] = key ?? "";
      else
        LastAssignedPrimary.Add(table ?? "", key ?? "");

      LocalCache.Upsert(table ?? "", new TableMetaData
      {
        LastAssignedValue = key,
        TableName = table
      });
      return true;
    }
    catch (Exception ex)
    {
      Notifier.NotifyError($"Saving New Id Error\r\n{ex.Message}", "Saving New Id Error", ex);
    }
    return false;
  }

  public string TryNextId<T>() where T : BaseModel => TryNextId(typeof(T));

  public string TryNextId(Type type)
  {
    var tt = type?.Name?.GetModelName();
    if (!type?.IsAssignableTo(typeof(BaseModel)) ?? false)
      throw new InvalidCastException("Only models inheriting from 'IBaseModel' are allowed in the database context");

    if ((type?.IsInterface) ?? false && tt.StartsWith("I"))
    {
      tt = tt?[1..];
      if (tt?.EndsWith("Model") ?? false)
        tt = tt[0..^5];
    }
    return TryNextId(tt ?? "");
  }

  public string TryNextId(string? table)
  {
    lock (Declarations.ApplicationDataPath)
    {
      if (string.IsNullOrWhiteSpace(table))
        throw new NullReferenceException("Unable to get table name");

      try
      {
        table = table.StrimLineTableName()?.Pluralize().MakeName().ToLower();

        if (LastAssignedPrimary.ContainsKey(table ?? ""))
          return GetNewId(LastAssignedPrimary[table ?? ""]);

        var id = LocalCache.Get<TableMetaData>(table ?? "")?.LastAssignedValue;

        try
        {
          if (id != null && !id.StartsWith(Declarations.DeviceNumber))
          {
            if (IdNames.ContainsKey(table ?? ""))
            {
              var tblName = IdNames[table ?? ""];
              var sql = PrimaryKeysSqls[tblName];

              if (Functions.ResolveObject<AppDbContext>() is not AppDbContext db)
                throw new InvalidOperationException("Unable to create data context");
              using var ds = Functions.GetDbDataSet(db.Database.GetDbConnection(), sql ?? "");
              id = ds.Tables[0].Rows[0]["CKey"]?.ToString() ?? $"{Declarations.DeviceNumber}000";
            }
          }
          LastAssignedPrimary.Add(table ?? "", id ?? "");
        }
        catch { }
        return id == null ? $"{prefix}-01" : GetNewId(id);
      }
      catch (Exception ex)
      {
        Notifier.NotifyError($"Getting next id error\r\n{ex.Message}", "Getting next id error", ex);
      }
    }
    return "";
  }

  /// <summary>
  /// The ReplaceFirstOccurrence.
  /// </summary>
  /// <param name="Source">The Source<see cref="string"/>.</param>
  /// <param name="Find">The Find<see cref="string"/>.</param>
  /// <param name="Replace">The Replace<see cref="string"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  private string ReplaceFirstOccurrence(string Source, string Find, string Replace)
  {
    try
    {
      int Place = Source.IndexOf(Find);
      var result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
      return result;
    }
    catch (Exception)
    {
      return Source;
    }
  }

  /// <summary>
  /// The ReplaceLastOccurrence.
  /// </summary>
  /// <param name="Source">The Source<see cref="string"/>.</param>
  /// <param name="Find">The Find<see cref="string"/>.</param>
  /// <param name="Replace">The Replace<see cref="string"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  private string ReplaceLastOccurrence(string Source, string Find, string Replace)
  {
    try
    {
      int Place = Source.LastIndexOf(Find);
      var result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
      return result;
    }
    catch (Exception)
    {
      return Source;
    }
  }

  public int? GetLastNumber(string input) =>
      Regex.Matches(input ?? "", @"\d+").OfType<Match>()
      .Select(m => (int?)int.Parse(m.Value)).LastOrDefault();

  /// <summary>
  /// The GetNewId.
  /// </summary>
  /// <param name="tableName">The tableName<see cref="string"/>.</param>
  /// <param name="lastId">The lastId<see cref="string"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  public string GetNewId(string lastId)
  {
    var id = "";
    try
    {
      if ((string.IsNullOrWhiteSpace(prefix) || prefix == "AAA") &&
          !string.IsNullOrWhiteSpace(Declarations.DeviceNumber))
        prefix = Declarations.DeviceNumber;

      prefix = (prefix ?? "AAA").Substring(0, 3);

      var lastIdObjs = lastId?.Split('-');

      if (string.IsNullOrWhiteSpace(lastId) || lastId?.Trim()?.Length < 1)
        id = $"{prefix}-01";
      else
      {
        var input = lastId;
        var number = GetLastNumber(input ?? "");
        id = number == null ?
            $"{prefix}-01" :
            ReplaceLastOccurrence(input ?? "", number?.ToString("00") ?? "", (number + 1)?.ToString("00") ?? "");
      }
    }
    catch { }
    return id;
  }
}
