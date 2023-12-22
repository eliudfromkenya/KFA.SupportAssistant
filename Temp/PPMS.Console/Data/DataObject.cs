using Humanizer;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Newtonsoft.Json;
using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using DatabaseTable = PPMS.Console.Models.DatabaseTable;

namespace PPMS.Console.Data
{
    public class Data
    {
        public XTable[] Tables { get; set; }
        public XColumn[] Columns { get; set; }
        public XPrimary[] Primaries { get; set; }
        public XRelation[] Relations { get; set; }
        public Dictionary<string, DataTable> TableDatas { get; set; }
    }
    static class DataObject
    {
        internal static void CheckToReset()
        {
            System.Console.WriteLine("Do you want to reset?");
            if (true || bool.TryParse(System.Console.ReadLine(), out bool ans) && ans)
            {
                try
                {
                    var mainPath = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "../../../..")).FullName;

                    var fileData = Path.Combine(mainPath, "Entries Table.json");
                    var text = File.ReadAllText(fileData);
                    var dd = JsonConvert.DeserializeObject<Data>(text);

                    if (dd?.Tables?.Any() == true)
                    {
                        using var db = new Context();
                        db.Tables.RemoveRange(db.Tables);
                        db.Columns.RemoveRange(db.Columns);
                        db.PrimaryKeys.RemoveRange(db.PrimaryKeys);
                        db.Relations.RemoveRange(db.Relations);
                        db.InitialData.RemoveRange(db.InitialData);
                        db.Groups.RemoveRange(db.Groups);
                        db.SaveChanges();

                        var tables = dd.Tables
                        .Select(x =>
                        {
                            var table = new DatabaseTable
                            {
                                Description = x.Description,
                                Id = x.TableId,
                                Name = x.Name.Pluralize(),
                                OriginalName = x.Name.Pluralize()
                            };

                            var group = new DataGroup
                            {
                                GroupName = "General",
                                Id = x.TableId,
                                ImagePath = "Main",
                                TableId = x.TableId
                            };
                            return new { table, group };
                        });


                        db.Tables.AddRange(tables.Select(x => x.table));
                        db.Groups.AddRange(tables.Select(x => x.group));
                        db.SaveChanges();


                        db.Columns.AddRange(dd.Columns
                        .Select(x => new TableColumn
                        {
                            Description = x.Description,
                            Id = x.ColumnId,
                            ColumnName = x.ColumnName,
                            ColumnFlags = x.ColumnFlags,
                            IsUnique = x.IsUnique,
                            IsNullable = x.ColumnName == "Narration" || x.IsNullable,
                            HasDefault = x.HasDefault,
                            DataType = x.DataType,
                            Default = x.Default,
                            Length = x.ColumnName == "Narration" ? 500 :
                            x.Length,
                            OriginalColumnName = x.OriginalColumnName,
                            Position = x.Position,
                            TableId = dd.Tables.FirstOrDefault(y => y.OriginalName.Pluralize() == x.TableName.Pluralize()).TableId
                        }));
                        db.SaveChanges();


                        db.PrimaryKeys.AddRange(dd.Primaries
                        .Select(x => new TablePrimary
                        {
                            Id = x.PrimaryKeyId,
                            ColumnId = dd.Columns.FirstOrDefault(y => y.ColumnName == x.Name
                            && y.TableName.Pluralize() == x.TableName.Pluralize()).ColumnId
                        }));
                        db.SaveChanges();

                        var rels = dd.Relations.ToList();
                        var allPrimaries = db.PrimaryKeys.ToArray();

                        //var cols = rels.Where(x => x.MasterTableName == "General Ledgers Details").ToArray();

                        //var dds = cols.Select(x => $"{x.ForeignTableName}={x.ForeignColumnName}={x.MasterTableName}={x.MasterColumnName}").ToArray();


                        var processed = new List<string>();
                        rels.ForEach(x =>
                        {
                            var nm = $"{x.ForeignColumnName}=>{x.ForeignTableName}=>{x.MasterColumnName}=>{x.MasterTableName}";

                            if (processed.Contains(nm))
                                return;
                            processed.Add(nm);

                            var fKey = dd.Columns.FirstOrDefault(y => y.ColumnName == x.ForeignColumnName && y.TableName.Pluralize() == x.ForeignTableName.Pluralize()).ColumnId;
                            var mKey = dd.Columns.FirstOrDefault(y => y.ColumnName == x.MasterColumnName && y.TableName.Pluralize() == x.MasterTableName.Pluralize()).ColumnId;

                            var ok = allPrimaries.Any(x => x.ColumnId == mKey);
                            if (!ok /*&& allPrimaries.Any(x => x.ColumnId == fKey)*/)
                            {
                                var d = mKey;
                                mKey = fKey;
                                fKey = d;
                            }
                            if (fKey != null && mKey != null)
                                db.Relations.Add(new TableRelation
                                {
                                    ForeignColumnId = fKey,
                                    MasterColumnId = mKey,
                                    Id = x.ForeignKeyId
                                });
                        });

                        Functions.InitialData =
                            dd.TableDatas
                            .Where(x => x.Value != null)
                            .ToDictionary(x => x.Key, y => y.Value);

                        db.SaveChanges();

                        var tbls = db.Tables.ToArray();
                        var mx = Regex.Split(Groups.names, "'~\r\n'")
               .Select(x => Regex.Split(x, "'~'")).ToArray()
               .Select(x =>
               {
                   var tbl = tbls.FirstOrDefault(y => y.Name == x[1] || y.Name == x[2])?.Id;
                   return new DataGroup
                   {
                       Id = x[0],
                       GroupName = x[4],
                       StrimLinedName = x[1],
                       ImagePath = x[3],
                       TableId = tbl
                   };
               }).ToArray();
                        db.Groups.AddRange(mx);
                        db.SaveChanges();

                        var obj = db.Tables.FirstOrDefault(XColumn => XColumn.Name == "User Logins");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("..........Error..............\r\n\r\n" + ex.ToString());
                }
                //var tables = 
            }
        }
    }
}
