#region

using System.Data;

#endregion

namespace CreateMxApp.Data
{
  public interface IRawData
  {
    Func<string, string> CheckTableName { get; set; }
    Func<string, string> CheckColumnName { get; set; }

    // Dictionary<string, string[][]> GetUniqueCols();
    string[] GetAllExistingDatabases();

    IDbTable[] GetTables();

    IDbTable[] GetQueries();

    IDbAppKey[] GetAppKeys();

    IDbColumn[] GetColumns();

    IDbColumn[] GetQueryColumns();

    IDbRelation[] GetRelations();

    Dictionary<string, int> GetTablesThatContainsData();

    DataTable GetTablesData(string? tableName);
  }
}
