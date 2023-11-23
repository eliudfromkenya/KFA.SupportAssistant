namespace KFA.SupportAssistant.Globals.DataLayer;

public interface IIdGenerator
{
  int? GetLastNumber(string input);

  string GetNewId(string lastId);

  string GetNextId(string table);

  string GetNextId(Type type);

  string GetNextId<T>() where T : BaseModel;

  Task<bool?> RefreshKeysAsync(bool forceRefresh = false);

  object RevertBack(string id);

  bool SaveNewId<T>(string id);

  string TryNextId(string table);

  string TryNextId(Type type);

  string TryNextId<T>() where T : BaseModel;
}
