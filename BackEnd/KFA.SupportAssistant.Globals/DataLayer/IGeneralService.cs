namespace KFA.SupportAssistant.Globals.DataLayer;

public interface IGeneralService
{
  Task<TableMetaData[]> RefreshKeys(string prefix);
}
