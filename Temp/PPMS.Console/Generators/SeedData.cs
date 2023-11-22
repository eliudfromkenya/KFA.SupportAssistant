using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PPMS.Console.Data;
using PPMS.Console.Models;

namespace PPMS.Console.Generators
{
    public class SeedData
    {
        internal static void GenerateContexts()
        {
            try
            {
                var sb = new StringBuilder(@"
using Microsoft.EntityFrameworkCore;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
           namespace Pilgrims.Projects.Assistant.DataLayer.Data {
                public static class Seed {
                    ");
                var sbConts = new StringBuilder();
                var modelSb = new StringBuilder();
                var modelSb1 = new StringBuilder();
                var definition = new[] { new { Name = "", Type = "", Ordinal = 0 } };

                //var defaultData = OldData.DefaultData;
                using (var db = new Context())
                {
                    foreach (var data in Functions.InitialData
                        ?? new Dictionary<string, System.Data.DataTable>())
                    {
                        var table = data.Key;
                        if (string.IsNullOrWhiteSpace(table))
                            continue;

                        var tableObj = db.Tables.Include(x => x.Columns)
                            .FirstOrDefault(x => x.Name == table);

                        var tableId = tableObj?.Id;
                        if (tableId == null)
                            continue;

                        modelSb1.AppendFormat(@"
         {{
            using var trans = await db.Database.BeginTransactionAsync();  
            try
            {{
                var ids = db.{0}.Select(x => x.Id);
                var objsToInsert = Seed.{0}.Where(x => !ids.Contains(x.Id)).ToArray();
                if (objsToInsert.Any())
                {{
                      db.{0}.AddRange(objsToInsert);
                      await db.SaveChangesAsync();
                      await trans.CommitAsync();       
                }}
            }}
            catch (System.Exception ex)
            {{
                trans.Rollback();
                exception = ex;
            }}
            }}
", data.Key.MakeName());

                        modelSb.AppendFormat(@"            builder.Entity<{0}>().HasData(Seed.{1});
", Functions.Singularize(data.Key).MakeName(), data.Key.MakeName());

                        var colNames = data.Value.Columns.OfType<DataColumn>()
                            .Select(x => x.ColumnName.MakeName()).ToArray();
                        var cols = tableObj.Columns.Where(x => colNames.Contains(x.ColumnName.MakeName())).ToArray();
                        var dataObjs = data.Value.Select().Select(x => x.ItemArray).ToArray();
                        var typeName = Functions.Singularize(table).MakeName();
                        //     sb.AppendFormat (@"            builder.Entity<{0}> ()
                        // .HasData (
                        //     ", Functions.Singularize (table).MakeName ());
                        sb.AppendFormat(@"  internal static {0}[] {1} = new {0}[]{{
                                ", typeName, table.MakeName());
                        sbConts.AppendFormat(@"db.{1}.AddRange (PPMS.API.EntityConfigs.Seed.{1});
                                db.SaveChanges ();
                                ", typeName, table.MakeName());

                        var primary = db.PrimaryKeys.Include(x => x.Column).FirstOrDefault(x => x.Column.TableId == tableId).Column.ColumnName;

                        var relCols = db.Relations.Include(x => x.ForeignColumn)
                        .Where(x => x.ForeignColumn.TableId == tableId)
                        .Select(x => x.ForeignColumn.ColumnName).Union(
                            db.Relations.Include(x => x.MasterColumn)
                        .Where(x => x.MasterColumn.TableId == tableId)
                        .Select(x => x.MasterColumn.ColumnName)
                        ).Distinct().ToArray();

                        string[] numCols = { "NumberOfDecimals", "Length" };
                        foreach (DataRow row in data.Value.Rows)
                        {
                            sb.AppendFormat("new {0}{{", typeName);
                            for (int i = 0; i <data.Value.Columns.Count; i++)
                            {
                                DataColumn col = data.Value.Columns[i];

                                var column = tableObj.Columns
                                    .FirstOrDefault(x => x.ColumnName.MakeName()
                                    == col.ColumnName.MakeName());

                                if (column == null)
                                    continue;

                                if (row[col] is DBNull || row[col] == null)
                                    continue;

                                var type = col.DataType;// Type.GetType(column.Type);

                                var colName = col.ColumnName;
                                if (col.ColumnName == primary)
                                    colName = "SetId";

                                var isRelCol = relCols.Contains(col.ColumnName);

                                colName = colName.MakeName();
                                if (isRelCol || col.ColumnName == primary)
                                {
                                    sb.AppendFormat(@" {0} = @""AAA-{1}""", colName, Convert.ToInt32(row[col]).ToString("00"));
                                }
                                else if (type == typeof(string)
                                    && !numCols.Contains(colName))
                                {
                                    sb.AppendFormat(@" {0} = @""{1}""", colName, row[col]);
                                }
                                else if (type == typeof(bool))
                                {
                                    sb.AppendFormat(@" {0} = {1}", colName, row[col].ToString().ToLower());
                                }
                                else if (type == typeof(DateTime))
                                {
                                    sb.AppendFormat(@" {0} = global::System.DateTime.TryParse(""{0}"", out global::System.DateTime date) ? date : new global::System.DateTime(1, 1, 1)", colName, row[col].ToString().ToLower());
                                }
                                else
                                {
                                    sb.AppendFormat(@" {0} = {1}", colName, row[col]);
                                }

                                if (i != cols.Length - 1)
                                    sb.Append(",");

                                // System.Console.WriteLine (table + "=>" + cols[i].Name + " => " + cols[i].Type + " = " + dta[i]);
                            }
                            sb.Append("}");
                            //if (dta != dataObjs.Last())
                            sb.Append(",");
                            sb.AppendLine();
                        }
                        sb.Append("};").AppendLine().AppendLine().AppendLine();

                        //var cols = JsonConvert.DeserializeAnonymousType<object[]> (data.Columns);

                    }
                }
                sb.Append(@" 
                }
            }");

                var modelString = $@"using Microsoft.EntityFrameworkCore;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.DataLayer.Data
{{
    public static class SeedData
    {{
        public static void SeedObjs(ModelBuilder builder)
        {{
            AddData(builder);
        }}

        private static void AddData(ModelBuilder builder)
        {{
{modelSb};
        }}

        internal async static Task<bool> CheckDefaultDate()
        {{
            Exception exception = null;
            using var db = Data.DataContext.Create();
           // using var trans = await db.Database.BeginTransactionAsync();
{modelSb1}   
            if (exception != null)
               throw Contracts.Functions.ExtractInnerException(exception);
            return true;
        }}
    }}
}}";
                var path = Path.Combine(Defaults.MainPath, "Data");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var xPath = Path.Combine(path, "Seed.cs");
                File.WriteAllText(xPath, sb.ToString());

                xPath = Path.Combine(path, "XSeed.cs");
                File.WriteAllText(xPath, sbConts.ToString());

                xPath = Path.Combine(path, "SeedData.cs");
                File.WriteAllText(xPath, modelString);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
        internal static void GenerateContext()
        {
            //            try
            //            {
            //                var sb = new StringBuilder(@"namespace PPMS.API.src.EntityConfigs {
            //    public static class Seed {
            //        ");
            //                var sbConts = new StringBuilder();

            //                var definition = new[] { new { Name = "", Type = "", Ordinal = 0 } };

            //                //var defaultData = OldData.DefaultData;
            //                using (var db = new Context())
            //                {
            //                    foreach (var data in defaultData)
            //                    {
            //                        var table = db.Tables.FirstOrDefault(x => x.Id == data.TableId)?.Name;
            //                        if (string.IsNullOrWhiteSpace(table))
            //                            continue;

            //                        var cols = JsonConvert.DeserializeAnonymousType(data.Columns, definition);
            //                        var dataObjs = JsonConvert.DeserializeObject<object[][]>(data.Data);
            //                        var typeName = Functions.Singularize(table).MakeName();
            //                        //     sb.AppendFormat (@"            builder.Entity<{0}> ()
            //                        // .HasData (
            //                        //     ", Functions.Singularize (table).MakeName ());
            //                        sb.AppendFormat(@"  internal static {0}[] {1} = new {0}[]{{
            //                    ", typeName, table.MakeName());
            //                        sbConts.AppendFormat(@"db.{1}.AddRange (PPMS.API.EntityConfigs.Seed.{1});
            //                    db.SaveChanges ();
            //                    ", typeName, table.MakeName());

            //                        var primary = db.PrimaryKeys.Include(x => x.Column).FirstOrDefault(x => x.Column.TableId == data.TableId).Column.ColumnName;

            //                        var relCols = db.Relations.Include(x => x.ForeignColumn)
            //                        .Where(x => x.ForeignColumn.TableId == data.TableId)
            //                        .Select(x => x.ForeignColumn.ColumnName).Union(
            //                            db.Relations.Include(x => x.MasterColumn)
            //                        .Where(x => x.MasterColumn.TableId == data.TableId)
            //                        .Select(x => x.MasterColumn.ColumnName)
            //                        ).Distinct().ToArray();

            //                        foreach (var dta in dataObjs)
            //                        {
            //                            sb.AppendFormat("new {0}{{", typeName);
            //                            for (int i = 0; i <cols.Length; i++)
            //                            {
            //                                var colData = dta[i];
            //                                if (colData == null)
            //                                    continue;

            //                                var type = Type.GetType(cols[i].Type);

            //                                var colName = cols[i].Name;
            //                                if (cols[i].Name == primary)
            //                                    colName = "SetId";

            //                                var isRelCol = relCols.Contains(cols[i].Name);

            //                                colName = colName.MakeName();
            //                                if (isRelCol || type == typeof(string))
            //                                {
            //                                    sb.AppendFormat(@" {0} = @""{1}""", colName, colData);
            //                                }
            //                                else if (type == typeof(bool))
            //                                {
            //                                    sb.AppendFormat(@" {0} = {1}", colName, colData.ToString().ToLower());
            //                                }
            //                                else if (type == typeof(DateTime))
            //                                {
            //                                    sb.AppendFormat(@" {0} = global::System.DateTime.TryParse(""{0}"", out global::System.DateTime date) ? date : new global::System.DateTime(1, 1, 1)", colName, colData.ToString().ToLower());
            //                                }
            //                                else
            //                                {
            //                                    sb.AppendFormat(@" {0} = {1}", colName, colData);
            //                                }

            //                                if (i != cols.Length - 1)
            //                                    sb.Append(",");

            //                                // System.Console.WriteLine (table + "=>" + cols[i].Name + " => " + cols[i].Type + " = " + dta[i]);
            //                            }
            //                            sb.Append("}");
            //                            if (dta != dataObjs.Last())
            //                                sb.Append(",");
            //                            sb.AppendLine();
            //                        }
            //                        sb.Append("};").AppendLine().AppendLine().AppendLine();

            //                        //var cols = JsonConvert.DeserializeAnonymousType<object[]> (data.Columns);

            //                    }
            //                }
            //                sb.Append(@" 
            //    }
            //}");

            //                var path = Path.Combine(Defaults.MainPath, "Data");
            //                if (!Directory.Exists(path))
            //                    Directory.CreateDirectory(path);

            //                var xPath = Path.Combine(path, "Seed.cs");
            //                File.WriteAllText(xPath, sb.ToString());

            //                xPath = Path.Combine(path, "XSeed.cs");
            //                File.WriteAllText(xPath, sbConts.ToString());
            //            }
            //            catch (Exception ex)
            //            {
            //                System.Console.WriteLine(ex.ToString());
            //            }
        }
    }
}