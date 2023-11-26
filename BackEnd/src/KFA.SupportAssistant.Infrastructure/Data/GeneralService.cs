using System.Data;
using KFA.SupportAssistant;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Pilgrims.Systems.Assistant.DataLayer.General;

public class GeneralService : IGeneralService
{
  public async Task<TableMetaData[]> RefreshKeys(string prefix)
  {
    try
    {
      if (Functions.ResolveObject<AppDbContext>() is not AppDbContext db)
        throw new InvalidOperationException("Unable to create data context");

      var sql = string.Join("\r\nUNION DISTINCT ", db.Model.GetEntityTypes()
          .Select(x =>
          {
            var tableName = x.GetTableName();
            var pri = x?.FindPrimaryKey()?.Properties.Select(x => x.GetColumnName(Microsoft.EntityFrameworkCore.Metadata.StoreObjectIdentifier.Table(tableName ?? "", null))).First();
            var attribs = new { Table = x?.GetTableName(), Key = pri, Type = x?.ClrType.Name };
            return attribs;
          })
          .Select(x =>
           string.Format(@"SELECT '{1}' as Name, '{3}' as type, (SELECT MAX({0}) FROM {1}
                                WHERE {0} LIKE '%{2}%' AND LENGTH({0}) = (SELECT MAX(LENGTH({0})) FROM {1} WHERE {0} LIKE '%{2}%')) as CKey",
                          x.Key, x.Table, prefix, x.Type)));

      using var con = db.Database.GetDbConnection();
      if (con.State != ConnectionState.Open) con.Open();

      var tables = new List<TableMetaData>();

      Declarations.DbLogger?.Information(sql);
      try
      {
        using var cmd = con.CreateCommand();
        cmd.CommandText = sql;
        lock (con)
        {
          using var reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            //var value = reader.GetValue(2)?.ToString();
            var tblName = reader.GetValue(0)?.ToString();
            var type = reader.GetValue(1)?.ToString();
            var value = reader.GetValue(2)?.ToString();
            //if (!string.IsNullOrWhiteSpace(value))
            {
              var obj = new TableMetaData
              {
                TableName = tblName,
                Type = type,
                LastAssignedValue = value
              };
              tables.Add(obj);
            }
          }
        }
      }
      catch { }
      return await Task.FromResult(tables.ToArray());
    }
    catch (Exception)
    {
      throw;
    }
  }
}
