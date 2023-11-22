//using PPMS.Console.Models;
//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Text;
//using System.Text.RegularExpressions;
//using Microsoft.Data.OleDb;
//using Microsoft.EntityFrameworkCore;
//using PPMS.Console.Generators;
//using System.IO;

//namespace PPMS.Console
//{
//    static class CheckCurrentDb
//    {
//        internal static void CheckData()
//        {
//            try
//            {
//                var path = Path.Combine(
//                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Entries Table.mdb");
//                var conString = string.Format($@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Persist Security Info=False;");
//                using var con = new OleDbConnection(conString);
//                using var db = new Data.Context();
//                var tableNames = new Dictionary<string, string>();
//                var addedTables = new List<string>();
//                using var cmd = con.CreateCommand();
//                var colSb = new StringBuilder("Added Tables");

//                var sql = @"SELECT tbl_name FROM OleDb_master WHERE type = 'table' AND NOT substr(tbl_name, 1,1) == '_'";
//                cmd.CommandText = sql;
//                con.Open();
//                using (var reader = cmd.ExecuteReader())
//                {
//                    int rowId = 300;
//                    while (reader.Read())
//                    {
//                        var tbl = reader.GetValue(0).ToString();
//                        var name = CheckColumn(tbl);
//                        string[] imuneTables = { "System Metadata", "OleDb Sequence", "Thombstone Deleted Records", "Table Interdependancies" };
//                        if (!imuneTables.Contains(name))
//                        {
//                            tableNames.Add(tbl, name);
//                            if (!db.Tables.Any(x => x.OriginalName.ToLower() == name.ToLower()))
//                            {
//                                db.Tables.Add(new DatabaseTable
//                                {
//                                    Id = (rowId++).ToString(),
//                                    Name = name,
//                                    OriginalName = name
//                                });
//                                addedTables.Add(name);
//                                colSb.AppendLine().AppendFormat("{0} => {1}", rowId - 1, name);
//                            }
//                        }
//                    }
//                    db.SaveChanges();
//                }

//                var tables = db.Tables.Include(x => x.Columns).ToArray();
//                var colChanges = new Dictionary<string, string>
//                        {
//                            { "Data Originator Id", "Originator Id" },
//                             { "Parent Epic Id", "Parent Ep Id" },
//                              { "Default User Translation Type Id", "Default User Translation Ty Id" },
//                               { "Activity Enum Number Id", "Activity Enum Number" },
//                                { "Type Of Relationship Id", "Type Of Relationsh Id" },
//                                 { "Parent Requirement Id", "Parent Requireme Id" },
//                           };

//                string[] immuneCols = { "Date Added", "Date Updated", "Originator Id" };

//                {

//                    foreach (var change in colChanges)
//                    {
//                        var cols = db.Columns.Where(x => x.ColumnName == change.Value);
//                        if (cols.Any())
//                        {
//                            cols.First().ColumnName = change.Key;
//                        }
//                    }

//                    db.SaveChanges();
//                }

//                int colRowId = 10000;
//                colSb.AppendLine().AppendLine().AppendLine().AppendLine("Added Columns");
//                foreach (var table in tableNames)
//                {
//                    cmd.CommandText = string.Format(@"SELECT * FROM {0}", table.Key);
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        TableColumn[] cols = { };
//                        DatabaseTable databaseTable = null;
//                        try
//                        {
//                            databaseTable = tables.First(x => x.Name == table.Value);
//                            cols = databaseTable?.Columns.ToArray();
//                        }
//                        catch (Exception)
//                        {
//                        }

//                        for (int i = 0; i <reader.FieldCount; i++)
//                        {
//                            var tt = CheckColumn(reader.GetName(i));
//                            if (immuneCols.Contains(tt))
//                                continue;

//                            if (!cols.Any(x => CheckColumn(x.ColumnName).ToLower().MakeName() == tt.ToLower().MakeName()))
//                            {
//                                var name = reader.GetDataTypeName(i);
//                                var type = reader.GetFieldType(i).ToString().Replace("System.", "").ToLower();
//                                if (type == "int64")
//                                    type = "bool";
//                                if (tt.EndsWith(" Id"))
//                                    type = "long";
//                                if (tt.ToLower().Contains("time") || tt.ToLower().Contains("date"))
//                                    type = "DateTime";

//                                var typObj = Functions.ReverseTypes(type);

//                                colSb.AppendLine().AppendFormat("{0} => {1}={2}={3}=({4})", table.Value, tt, name, type, typObj);
//                                db.Columns.Add(new TableColumn
//                                {
//                                    Id = colRowId.ToString(),
//                                    ColumnName = tt,
//                                    DataType = typObj,
//                                    IsNullable = true,
//                                    OriginalColumnName = tt,
//                                    TableId = databaseTable?.Id
//                                });
//                            }
//                            //is unique, nullable, length, is indexed, default
//                            if (addedTables.Contains(table.Value) && i == 0)
//                            {
//                                db.PrimaryKeys.Add(new TablePrimary { ColumnId = colRowId.ToString(), Id = colRowId.ToString() });
//                                colSb.Append(" As Primary Key");
//                            }
//                            colRowId++;
//                        }
//                    }
//                    db.SaveChanges();
//                }

//                sql = @"SELECT
//                        m.name
//                        , p.*
//                    FROM
//                        OleDb_master m
//                        JOIN pragma_foreign_key_list(m.name) p ON m.name != p.""table""
//                    WHERE m.type = 'table'
//                    ORDER BY m.name;";

//                {
//                    cmd.CommandText = sql;
//                    con.Open();
//                    var relList = new List<Tuple<string, string, string, string>>();
//                    var cols = db.Columns.Include(x => x.Table).ToArray();
//                    using (var reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            var foreign = CheckColumn(reader.GetValue(reader.GetOrdinal("name")));
//                            var master = CheckColumn(reader.GetValue(reader.GetOrdinal("table")));
//                            var foreignColumn = CheckColumn(reader.GetValue(reader.GetOrdinal("from")));
//                            var masterColumn = CheckColumn(reader.GetValue(reader.GetOrdinal("to")));
//                            var foreignCol = cols.FirstOrDefault(x =>
//                            {
//                                return x.ColumnName.MakeName().ToLower() == foreignColumn.MakeName().ToLower() &&
//                                x.Table?.Name.MakeName().ToLower() == foreign.MakeName().ToLower();
//                            });
//                            var masterCol = cols.FirstOrDefault(x =>
//                            {
//                                return x.ColumnName.MakeName().ToLower() == masterColumn.MakeName().ToLower() &&
//                                x.Table?.Name.MakeName().ToLower() == master.MakeName().ToLower();
//                            });

//                            relList.Add(new Tuple<string, string, string, string>(foreignCol?.Id, masterCol?.Id, foreign + "=> " + foreignColumn, master + "=> " + masterColumn));
//                        }
//                    }

//                    {
//                        var rels = db.Relations.ToArray();

//                        var missing = relList.Except(relList
//                            .Where(x => rels.Any(yx => yx.MasterColumnId == x.Item2 && yx.ForeignColumnId == x.Item1)))
//                            .ToList();

//                        colRowId = 1000;
//                        colSb.AppendLine().AppendLine().AppendLine().AppendLine("Adding Relations");
//                        foreach (var rel in missing)
//                        {
//                            db.Relations.Add(new TableRelation
//                            {
//                                Id = colRowId++.ToString(),
//                                ForeignColumnId = rel.Item1,
//                                MasterColumnId = rel.Item2
//                            });
//                            colSb.AppendLine().AppendFormat("{1}={2}=>{3}={4}", "", rel.Item1, rel.Item2, rel.Item3, rel.Item4);
//                        }
//                        db.SaveChanges();
//                    }
//                }

//                var path = "";
//                if (!Directory.Exists(Defaults.MainPath))
//                    Directory.CreateDirectory(Defaults.MainPath);

//                path = Path.Combine(Defaults.MainPath, "Added Data.txt");
//                File.WriteAllText(path, colSb.ToString());
//                global::System.Console.WriteLine("Done getting additional data updates");

//            }
//            catch (Exception ex)
//            {
//                global::System.Console.WriteLine(ex);
//            }
//        }

//        private static string CheckColumn(object value)
//        {
//            var name = value.ToString().Replace("sys_", "").Replace("tbl_", "").Replace("_", " ").Trim();
//            foreach (Match mat in Regex.Matches(name, "[A-Z]"))
//            {
//                name = name.Replace(mat.Value, " " + mat.Value);
//            }
//            name = Functions.MakeAllFirstLetterCapital(name, false);
//            return name.Trim();
//        }
//    }
//}
