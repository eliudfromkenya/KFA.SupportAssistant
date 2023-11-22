#region

using System.Collections.Generic;
using System.Data;

#endregion

namespace Pilgrims.ProjectManagement.Contracts.DataImport
{
    public interface IRawData
    {
        string ConnectionString { get; }
        bool TryToExctractWords { get; set; }
        IDbTable[] GetTables();
        IDbPrimaryKeys[] GetPrimaryKeys();
        IDbColumn[] GetColumns();
        IDbRelations[] GetRelations();
        Dictionary<string, int> GetTablesThatContainsData();
        DataTable GetTablesData(string tableName);
    }
}