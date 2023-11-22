using System.Data;
using CreateMxApp.Data;
using Humanizer;
using OfficeOpenXml;

namespace CreateMxApp.Services;

public class MsAccessRawData : IRawData
{
  private List<(string[] names, string? type)> tableNames = new();

  public MsAccessRawData(string? fileName)
  {
    FileName = fileName;
    Initialize();
  }

  public MsAccessRawData(string? fileName, string? username, string? password)
  {
    FileName = fileName;
    Username = username;
    Password = password;
    Initialize();
  }

  public Func<string, string> CheckTableName { get; set; }
  public Func<string, string> CheckColumnName { get; set; }
  public string? FileName { get; }
  public string? Username { get; }
  public string? Password { get; }

  public OleDbConnection GetConnection() => new($@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={FileName};Persist Security Info=False;");

  public string[] GetAllExistingDatabases()
  {
    return new[] { FileName };
  }

  private readonly Dictionary<string, int> Ids = new();
  private IDbColumn[] _columns;
  private IDbAppKey[] _primaryKeys;
  private IDbRelation[] _relations;
  private IDbTable[] _tables;
  private Dictionary<string, int> _tableRowCount;

  private string? GetId(string? name)
  {
    if (Ids.ContainsKey(name))
    {
      return $"AAA-{Ids[name]++:00}";
    }
    else
    {
      Ids.Add(name, 2);
      return "AAA-01";
    }
  }

  private string? StreamLineTableName(object name)
  {
    if (CheckTableName != null)
      name = CheckTableName(name?.ToString());

    return name?.ToString().Titleize();
  }

  private string? StreamLineColumnName(object name)
  {
    if (CheckColumnName != null)
      name = CheckColumnName(name?.ToString());

    return name?.ToString().Titleize();
  }

  private static (string? Id, TypeCode TypeCode) GetDataType(string? name)
  {
    if (!int.TryParse(name?.Trim(), out int type))
      return ("AAA-15", TypeCode.String);

    (string, TypeCode) mm = type switch
    {
      2 => ("AAA-03", TypeCode.Single),
      3 => ("AAA-02", TypeCode.Int64),
      4 => ("AAA-01", TypeCode.Single),
      5 => ("AAA-04", TypeCode.Double),
      7 => ("AAA-06", TypeCode.DateTime),
      11 => ("AAA-09", TypeCode.Boolean),
      17 => ("AAA-10", TypeCode.Byte),
      72 => ("AAA-11", TypeCode.String),
      129 or 133 => ("AAA-13", TypeCode.Byte),
      130 or 134 => ("AAA-15", TypeCode.String),
      8 => ("AAA-17", TypeCode.Int32),
      128 => ("AAA-12", TypeCode.Byte),
      6 or 131 => ("AAA-29", TypeCode.Decimal),
      15 => ("AAA-30", TypeCode.Char),
      _ => ("AAA-15", TypeCode.String),
    };
    return mm;
  }

  public IDbColumn[] GetColumns()
  {
    AddExcelTable();

    lock (FileName)
    {
      if (_columns == null)
        using (var cn = GetConnection())
        {
          cn.Open();
          var restrictions = new string?[] { null };
          using var schema = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);
          using var oleDbSchemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes,
              new object[] { null, null, null, null, null });

          if (oleDbSchemaTable != null)
          {
            var uniques =
                oleDbSchemaTable
                    .Select()
                    .Select(x => new
                    {
                      Table = x["Table_Name"].ToString(),
                      Column = x["Column_Name"].ToString(),
                      Unique = (bool)x["UNIQUE"]
                    })
                    .GroupBy(z => z.Column)
                    .Where(y => y.Count() < 2)
                    .Select(a => a.First())
                    .Where(b => b.Unique).ToArray();

            _columns = schema.Select(
                    @"Table_Name IS NOT NULL AND Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
                .Select(rs => new DbColumn
                {
                  ColumnId = GetId("Columns"),
                  TableName = (rs["TABLE_NAME"] is DBNull
                            ? ""
                            : rs["TABLE_NAME"].ToString().Titleize()),
                  OriginalColumnName = rs["COLUMN_NAME"].ToString(),
                  IsUnique = uniques.Any(
                        x => x.Column.Equals(rs["COLUMN_NAME"].ToString()) &&
                             x.Table.Equals(rs["TABLE_NAME"].ToString())),
                  Position = rs["ORDINAL_POSITION"] is DBNull
                        ? (byte)0
                        : Convert.ToByte(rs["ORDINAL_POSITION"].ToString()),
                  HasDefault = bool.TryParse(rs["COLUMN_HASDEFAULT"].ToString(), out bool bl) && bl,
                  Default = rs["COLUMN_DEFAULT"] is DBNull ? "" : rs["COLUMN_DEFAULT"].ToString(),
                  ColumnFlags = int.TryParse(rs["COLUMN_FLAGS"].ToString(), out int it) ? it : 0,
                  IsNullable = bool.TryParse(rs["IS_NULLABLE"].ToString(), out bl) && bl,
                  DataType = GetDataType(rs["DATA_TYPE"].ToString()),
                  Length = int.TryParse(rs["CHARACTER_MAXIMUM_LENGTH"].ToString(), out it)
                        ? it
                        : -1,
                  ColumnName =
                        (rs["COLUMN_NAME"] is DBNull
                            ? ""
                            : rs["COLUMN_NAME"].ToString().Titleize()),
                  Description = rs["Description"] is DBNull ? "" : rs["Description"].ToString()
                })
                .Where(m => (tableNames.FirstOrDefault(v => v.type.Equals("TABLE", StringComparison.OrdinalIgnoreCase)).names ?? Array.Empty<string>())
                .Contains(m.TableName))
                .ToArray();
          }
        }
    }

    return _columns;
  }

  public IDbTable[] GetQueries()
  {
    lock (FileName)
    {
      using var cn = GetConnection();
      cn.Open();
      var restrictions = new string[] { null };
      return
          cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions)
              .Select(
                  @"Table_Type = 'VIEW' AND Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
              .Select(rs => new DbTable
              {
                TableId = GetId("Table"),
                Name = rs["TABLE_NAME"].ToString()?.Titleize(),
                OriginalName = rs["TABLE_NAME"].ToString(),
                Description = rs["Description"].ToString()
              })
              .ToArray();
    }
  }

  public IDbColumn[] GetQueryColumns()
  {
    AddExcelTable();

    lock (FileName)
    {
      using var cn = GetConnection();
      cn.Open();
      var restrictions = new string[] { null };
      using var schema = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);
      using var oleDbSchemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes,
          new object[] { null, null, null, null, null });

      var uniques =
          oleDbSchemaTable
              .Select()
              .Select(x => new
              {
                Table = x["Table_Name"].ToString(),
                Column = x["Column_Name"].ToString(),
                Unique = (bool)x["UNIQUE"]
              })
              .GroupBy(z => z.Column)
              .Where(y => y.Count() < 2)
              .Select(a => a.First())
              .Where(b => b.Unique).ToArray();

      return schema.Select(
              @"Table_Name IS NOT NULL AND Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
          .Select(rs => new DbColumn
          {
            ColumnId = GetId("Columns"),
            TableName = (rs["TABLE_NAME"] is DBNull
                      ? ""
                      : rs["TABLE_NAME"].ToString().Titleize()),
            OriginalColumnName = rs["COLUMN_NAME"].ToString(),
            IsUnique = uniques.Any(
                  x => x.Column.Equals(rs["COLUMN_NAME"].ToString()) &&
                       x.Table.Equals(rs["TABLE_NAME"].ToString())),
            Position = rs["ORDINAL_POSITION"] is DBNull
                  ? (byte)0
                  : Convert.ToByte(rs["ORDINAL_POSITION"].ToString()),
            HasDefault = bool.TryParse(rs["COLUMN_HASDEFAULT"].ToString(), out bool bl) && bl,
            Default = rs["COLUMN_DEFAULT"] is DBNull ? "" : rs["COLUMN_DEFAULT"].ToString(),
            ColumnFlags = int.TryParse(rs["COLUMN_FLAGS"].ToString(), out int it) ? it : 0,
            IsNullable = bool.TryParse(rs["IS_NULLABLE"].ToString(), out bl) && bl,
            DataType = GetDataType(rs["DATA_TYPE"].ToString()),
            Length = int.TryParse(rs["CHARACTER_MAXIMUM_LENGTH"].ToString(), out it)
                  ? it
                  : -1,
            ColumnName =
                  (rs["COLUMN_NAME"] is DBNull
                      ? ""
                      : rs["COLUMN_NAME"].ToString().Titleize()),
            Description = rs["Description"] is DBNull ? "" : rs["Description"].ToString()
          })
          .Where(m => (tableNames.FirstOrDefault(v => v.type.Equals("VIEW", StringComparison.OrdinalIgnoreCase)).names ?? Array.Empty<string>())
          .Contains(m.TableName))
          .ToArray();
    }
  }

  public IDbAppKey[] GetAppKeys()
  {
    // var genId = GlobalDeclarations.DiContainer.Resolve<IIdentityGenerator>();
    if (_primaryKeys == null)
      using (var cn = GetConnection())
      {
        cn.Open();
        using var primaryKeys = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new string[] { null });
        List<DbAppKey> dbAppKeys = new();
        //cn.Open();
        dbAppKeys = primaryKeys
                .Select("PK_Name = 'PrimaryKey' AND Table_Name IS NOT NULL  AND Table_Name IS NOT NULL")
                .GroupBy(rs => rs["Table_Name"].ToString())
                .Select(rs => new DbAppKey
                {
                  KeyId = GetId("PrimaryKey"),
                  Columns = rs.Select(c => (c["COLUMN_NAME"].ToString()?.Titleize(), rs.Key?.Titleize())).ToArray(),
                  AppKeyType = AppKeyType.PrimaryKey,
                  Name = $"{rs.Key?.Titleize()} Primary Key"
                }).ToList();

        using var oleDbSchemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes,
                new object[] { null, null, null, null, null });

        if (oleDbSchemaTable != null)
        {
          var keys =
              oleDbSchemaTable
                  .Select()
                  .Select(x => new
                  {
                    IndexName = x["INDEX_NAME"].ToString(),
                    OrdinalPosition = int.TryParse(x["ORDINAL_POSITION"].ToString(), out int pos) ? pos : 0,
                    Table = x["Table_Name"].ToString(),
                    Column = x["Column_Name"].ToString(),
                    Unique = (bool)x["UNIQUE"]
                  }).OrderByDescending(m => m.IndexName?.Length)
                  .ToList();

          foreach (DataRow row in primaryKeys.Rows)
          {
            var key = keys.FirstOrDefault(v => v.Table == row["Table_Name"].ToString() && v.Column == row["Column_Name"].ToString() && v.IndexName == row["PK_NAME"].ToString());
            if (key != null)
              keys.Remove(key);
          }

          while (keys.Any())
          {
            var key = keys.First();
            if (key.IndexName?.Trim()?.EndsWith("+") ?? false)
            {
              var uniques = keys.Where(c => c.IndexName?.Replace("+", "") == key.IndexName?.Replace("+", "")).ToList();
              dbAppKeys.Add(new DbAppKey
              {
                AppKeyType = AppKeyType.UniqueKey,
                KeyId = GetId("PrimaryKey"),
                Name = key.IndexName,
                Columns = uniques.Select(v => (v.Column, v.Table)).ToArray()
              });
              uniques.ForEach(c => keys.Remove(c));
            }
            else
            {
              dbAppKeys.Add(new DbAppKey
              {
                AppKeyType = key.Unique ? AppKeyType.UniqueKey : AppKeyType.DbIndex,
                KeyId = GetId("PrimaryKey"),
                Name = key.IndexName,
                Columns = new[] { (key.Column, key.Table) }
              });
              keys.Remove(key);
            }
          }
        }
        _primaryKeys = dbAppKeys?.ToArray() ?? Array.Empty<DbAppKey>();
      }
    return _primaryKeys;
  }

  public IDbRelation[] GetRelations()
  {
    if (_relations == null)
      using (var cn = GetConnection())
      {
        cn.Open();
        _relations =
            cn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, Array.Empty<string>())
                .Select(
                    "FK_Table_Name IS NOT NULL AND PK_Table_Name IS NOT NULL AND PK_Column_Name IS NOT NULL AND FK_Column_Name IS NOT NULL")
                .Select(rs => new DbRelation
                {
                  ForeignKeyId = GetId("ForeignKey"),
                  MasterTableName =
                        rs["PK_Table_Name"].ToString()?.Titleize(),
                  MasterColumnName =
                        rs["PK_Column_Name"].ToString()?.Titleize(),
                  ForeignTableName =
                        rs["FK_Table_Name"].ToString()?.Titleize(),
                  ForeignColumnName =
                        rs["FK_Column_Name"].ToString()?.Titleize(),
                })
                .ToArray();
      }
    return _relations;
  }

  public void Initialize()
  {
    using var cn = GetConnection();
    cn.Open();
    var restrictions = new string[] { null };
    using var table = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
    tableNames = table
            .Select(
                @"Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
            .Select(rs => new
            {
              Name = rs["TABLE_NAME"].ToString(),
              Type = rs["TABLE_TYPE"].ToString()
            }).GroupBy(v => v.Type)
            .Select(v => (v.Select(m => m.Name).ToArray(), v.Key))
            .Where(m => new[] { "TABLE", "VIEW" }.Contains(m.Key, StringComparer.OrdinalIgnoreCase))
            .ToList();
  }

  private void AddExcelTable()
  {
    using var excelPackage = new ExcelPackage();
    //using var ws = excelPackage.Workbook.Worksheets.Add("Tables Data");
    using var cn = GetConnection();
    cn.Open();
    var restrictions = new string[] { null };
    using var columnsTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);
    using var indexesTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes,
        new object[] { null, null, null, null, null });
    using var primaryKeysTable =
                cn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new string[] { null });
    using var relationsTable =
                cn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, Array.Empty<string>());
    using var tablesTable =
                    cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);

    using var wsTables = excelPackage.Workbook.Worksheets.Add("Tables Data");
    using var wsColumns = excelPackage.Workbook.Worksheets.Add("Columns Data");
    using var wsPrimaryKeys = excelPackage.Workbook.Worksheets.Add("Primary Keys Data");
    using var wsRelationships = excelPackage.Workbook.Worksheets.Add("Relationship Data");
    using var wsIndexes = excelPackage.Workbook.Worksheets.Add("Index Data");

    if (tablesTable != null)
      wsTables.Cells["A1"].LoadFromDataTable(tablesTable, true);
    if (columnsTable != null)
      wsColumns.Cells["A1"].LoadFromDataTable(columnsTable, true);
    if (primaryKeysTable != null)
      wsPrimaryKeys.Cells["A1"].LoadFromDataTable(primaryKeysTable, true);
    if (relationsTable != null)
      wsRelationships.Cells["A1"].LoadFromDataTable(relationsTable, true);
    if (indexesTable != null)
      wsIndexes.Cells["A1"].LoadFromDataTable(indexesTable, true);

    var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"MsAccessImportedData.xlsx");
    excelPackage.SaveAs(new FileInfo(file));
  }

  public IDbTable[] GetTables()
  {
    //var genId = GlobalDeclarations.DiContainer.Resolve<IIdentityGenerator>();

    lock (FileName)
    {
      if (_tables == null)
        using (var cn = GetConnection())
        {
          cn.Open();
          var restrictions = new string[] { null };
          _tables =
              cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions)
                  .Select(
                      @"Table_Type = 'TABLE' AND Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
                  .Select(rs => new DbTable
                  {
                    TableId = GetId("Table"),
                    Name = rs["TABLE_NAME"].ToString()?.Titleize(),
                    OriginalName = rs["TABLE_NAME"].ToString(),
                    Description = rs["Description"].ToString()
                  })
                  .ToArray();
        }
    }
    return _tables;
  }

  public Dictionary<string, string[][]> GetUniqueCols()
  {
    return GetColumns().Where(x => x.IsUnique)
         .GroupBy(c => c.TableName)
         .ToDictionary(c => c.Key, y => new[] { y.Select(c => c.ColumnName).ToArray() });
  }

  public Dictionary<string, int> GetTablesThatContainsData()
  {
    if (_tableRowCount == null)
      using (var cn = GetConnection())
      {
        cn.Open();
        var restrictions = new string[] { null };
        var tbls =
            cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions)
                .Select(
                    @"Table_Type = 'TABLE' AND Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
                .Select(rs => new
                {
                  TableName = rs["Table_Name"].ToString(),
                  Description = rs["Description"].ToString()
                })
                .ToArray();

        using var cmd = cn.CreateCommand();
        _tableRowCount = tbls.Chunk(15)
            .Select(mm => string.Join(" UNION \r\n",
                mm.Select(x => string.Format("SELECT '{0}' as Name, COUNT(*) as [Count] FROM [{0}]",
                    x.TableName))))
            .Select(sql =>
            {
              var nCmd = cmd;
              nCmd.CommandText = sql;
              using var adp = new OleDbDataAdapter(nCmd);
              using var countTable = new DataTable();
              adp.Fill(countTable);
              var objs = countTable.Select("[Count] > 0")
                            .ToDictionary(
                                cc => cc["Name"].ToString().Titleize(),
                                yy => Convert.ToInt32(yy["Count"]));
              return objs;
            })
            .SelectMany(x => x).ToDictionary(x => x.Key, y => y.Value);
      }
    return _tableRowCount;
  }

  public DataTable GetTablesData(string? xTableName)
  {
    var xTable = GetTables().FirstOrDefault(x => x.OriginalName == xTableName);
    if (xTable == null)
      return new DataTable();

    var tableName = xTable.OriginalName;
    if (string.IsNullOrWhiteSpace(tableName))
      throw new Exception("Table name is required");

    using var cn = GetConnection();
    cn.Open();
    using var nCmd = cn.CreateCommand();
    nCmd.CommandText = string.Format("SELECT * FROM [{0}]", tableName);
    using var adp = new OleDbDataAdapter(nCmd);
    using var table = new DataTable();
    adp.Fill(table);
    return table;
  }
}
