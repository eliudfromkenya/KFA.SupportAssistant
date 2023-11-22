#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Pilgrims.ProjectManagement.Contracts.DataImport;
using Pilgrims.ProjectManagement.DataImport;
using Pilgrims.ProjectManagement.DataImport.Models;

#endregion

namespace InitProject.RawData.MsAccess
{
    public class RawData : IRawData
    {
        private readonly object _lockObject = new object();
        private IDbColumn[] _columns;
        private IDbPrimaryKeys[] _primaryKeys;
        private IDbRelations[] _relations;
        private Dictionary<string, int> _tableRowCount;
        private IDbTable[] _tables;
        static int mm = 1000;

        public RawData(string msAccessFilePath)
        {
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}",
                msAccessFilePath);
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public bool TryToExctractWords { get; set; }

        public IDbTable[] GetTables()
        {
            //var genId = GlobalDeclarations.DiContainer.Resolve<IIdentityGenerator>();         

            lock (_lockObject)
            {
                if (_tables == null)
                    using (var cn = new OleDbConnection(ConnectionString))
                    {
                        cn.Open();
                        var restrictions = new string[] { null };
                        _tables =
                            cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions)
                                .Select(
                                    @"Table_Type = 'TABLE' AND Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
                                .Select(rs => new DbTable
                                {
                                    TableId = mm++.ToString(),
                                    Name = Common.CheckNames(rs["TABLE_NAME"].ToString(), TryToExctractWords),
                                    OriginalName = rs["TABLE_NAME"].ToString(),
                                    Description = rs["Description"].ToString()
                                })
                                .ToArray();
                    }
            }
            return _tables;
        }

        public IDbPrimaryKeys[] GetPrimaryKeys()
        {
            // var genId = GlobalDeclarations.DiContainer.Resolve<IIdentityGenerator>();
            if (_primaryKeys == null)
                using (var cn = new OleDbConnection(ConnectionString))
                {
                    cn.Open();
                    _primaryKeys =
                        cn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new string[] { null })
                            .Select("PK_Name = 'PrimaryKey' AND Table_Name IS NOT NULL  AND Table_Name IS NOT NULL")
                            .Select(rs => new DbPrimaryKeys
                            {
                                PrimaryKeyId = mm++.ToString(),
                                Name = Common.CheckNames(rs["COLUMN_NAME"].ToString(), TryToExctractWords),
                                TableName = Common.CheckNames(rs["TABLE_NAME"].ToString(), TryToExctractWords)
                            }).ToArray();
                }
            return _primaryKeys;
        }

        public IDbColumn[] GetColumns()
        {
            //var genId = GlobalDeclarations.DiContainer.Resolve<IIdentityGenerator>();

            lock (_lockObject)
            {
                if (_columns == null)
                    using (var cn = new OleDbConnection(ConnectionString))
                    {
                        cn.Open();
                        var restrictions = new string[] { null };
                        using (var schema = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions))
                        {
                            var oleDbSchemaTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Indexes,
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

                                bool bl;
                                int it;
                                _columns = schema.Select(
                                        @"Table_Name IS NOT NULL AND Table_Name NOT LIKE '%~' AND Table_Name NOT LIKE '%tmpclp%'")
                                    .Select(rs => new DbColumn
                                    {
                                        ColumnId = mm++.ToString(),
                                        TableName =
                                            Common.CheckNames(rs["TABLE_NAME"] is DBNull
                                                ? ""
                                                : rs["TABLE_NAME"].ToString(), TryToExctractWords),
                                        OriginalColumnName = rs["COLUMN_NAME"].ToString(),
                                        IsUnique = uniques.Any(
                                            x => x.Column.Equals(rs["COLUMN_NAME"].ToString()) &&
                                                 x.Table.Equals(rs["TABLE_NAME"].ToString())),
                                        Position = rs["ORDINAL_POSITION"] is DBNull
                                            ? (byte)0
                                            : Convert.ToByte(rs["ORDINAL_POSITION"].ToString()),
                                        HasDefault = bool.TryParse(rs["COLUMN_HASDEFAULT"].ToString(), out bl) && bl,
                                        Default = rs["COLUMN_DEFAULT"] is DBNull ? "" : rs["COLUMN_DEFAULT"].ToString(),
                                        ColumnFlags = int.TryParse(rs["COLUMN_FLAGS"].ToString(), out it) ? it : 0,
                                        IsNullable = bool.TryParse(rs["IS_NULLABLE"].ToString(), out bl) && bl,
                                        DataType = int.TryParse(rs["DATA_TYPE"].ToString(), out it) ? it : -1,
                                        Length = int.TryParse(rs["CHARACTER_MAXIMUM_LENGTH"].ToString(), out it)
                                            ? it
                                            : -1,
                                        ColumnName =
                                            Common.CheckNames(rs["COLUMN_NAME"] is DBNull
                                                ? ""
                                                : rs["COLUMN_NAME"].ToString(), TryToExctractWords),
                                        Description = rs["Description"] is DBNull ? "" : rs["Description"].ToString()
                                    })
                                    .ToArray();
                            }
                        }
                    }
            }


            return _columns;
        }

        public IDbRelations[] GetRelations()
        {
            //var genId = GlobalDeclarations.DiContainer.Resolve<IIdentityGenerator>();
            if (_relations == null)
                using (var cn = new OleDbConnection(ConnectionString))
                {
                    cn.Open();
                    _relations =
                        cn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new string[] { })
                            .Select(
                                "FK_Table_Name IS NOT NULL AND PK_Table_Name IS NOT NULL AND PK_Column_Name IS NOT NULL AND FK_Column_Name IS NOT NULL")
                            .Select(rs => new DbRelations
                            {
                                ForeignKeyId = mm++.ToString(),
                                MasterTableName =
                                    Common.CheckNames(rs["PK_Table_Name"].ToString(), TryToExctractWords),
                                MasterColumnName =
                                    Common.CheckNames(rs["PK_Column_Name"].ToString(), TryToExctractWords),
                                ForeignTableName =
                                    Common.CheckNames(rs["FK_Table_Name"].ToString(), TryToExctractWords),
                                ForeignColumnName =
                                    Common.CheckNames(rs["FK_Column_Name"].ToString(), TryToExctractWords)
                            })
                            .ToArray();
                }
            return _relations;
        }


        public static List<T> Randomize<T>(IEnumerable<T> values)
        {
            var list = values as List<T> ?? values.ToList();
            var n = list.Count;
            var rnd = new Random();
            while (n > 1)
            {
                var k = rnd.Next(0, n) % n;
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        public static List<List<T>> Chunk<T>(IEnumerable<T> source, int size)
        {
            var chunks = new List<List<T>>();
            var enumerable = source as T[] ?? source.ToArray();
            var xCount = (enumerable.Length / size) + 2;

            for (var i = 0; i < xCount; i++)
            {
                var temp = enumerable.Skip(i * size).Take(size).ToList();
                if (temp.Any())
                    chunks.Add(temp);
            }
            return chunks;
        }


        public Dictionary<string, int> GetTablesThatContainsData()
        {
            if (_tableRowCount == null)
                using (var cn = new OleDbConnection(ConnectionString))
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


                    using (var cmd = cn.CreateCommand())
                    {
                        _tableRowCount = Chunk(tbls, 15)
                            .Select(mm => string.Join(" UNION \r\n",
                                mm.Select(x => string.Format("SELECT '{0}' as Name, COUNT(*) as [Count] FROM [{0}]",
                                    x.TableName))))
                            .Select(sql =>
                            {
                                var nCmd = cmd;
                                nCmd.CommandText = sql;
                                using (var adp = new OleDbDataAdapter(nCmd))
                                {
                                    using (var countTable = new DataTable())
                                    {
                                        adp.Fill(countTable);
                                        var objs = countTable.Select("[Count] > 0")
                                            .ToDictionary(
                                                cc => Common.CheckNames(cc["Name"].ToString(),
                                                    true),
                                                yy => Convert.ToInt32(yy["Count"]));
                                        return objs;
                                    }
                                }
                            })
                            .SelectMany(x => x).ToDictionary(x => x.Key, y => y.Value);
                    }
                }
            return _tableRowCount;
        }

        public DataTable GetTablesData(string xTableName)
        {
            var xTable = GetTables().FirstOrDefault(x => x.OriginalName == xTableName);
            if (xTable == null)
                return new DataTable();

            var tableName = xTable.OriginalName;
            if (string.IsNullOrWhiteSpace(tableName))
                throw new Exception("Table name is required");

            using (var cn = new OleDbConnection(ConnectionString))
            {
                cn.Open();
                using (var nCmd = cn.CreateCommand())
                {
                    nCmd.CommandText = string.Format("SELECT * FROM [{0}]", tableName);
                    using (var adp = new OleDbDataAdapter(nCmd))
                    {
                        using (var table = new DataTable())
                        {
                            adp.Fill(table);
                            return table;
                        }
                    }
                }
            }
        }
    }
}
