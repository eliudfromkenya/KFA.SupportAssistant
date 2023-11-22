using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Humanizer;
using InitProject.RawData.MsAccess;
using Newtonsoft.Json;
using PPMS.Console.Models;
using ProjectMaker.Data;

namespace ProjectMaker
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      string connectionString = string.Format("Provider={0}; Data Source={1}; Jet OLEDB:Engine Type={2}",
          "Microsoft.Jet.OLEDB.4.0",
          "mydb.mdb",
          5);

      ADOX.Catalog catalog = new ADOX.Catalog();
      catalog.Create(connectionString);

      ADOX.Table table = new ADOX.Table();
      table.Name = "Users";   // Table name

      // Column 1 (id)
      ADOX.Column idCol = new ADOX.Column
      {
        Name = "Id",  // The name of the column
        ParentCatalog = catalog,
        Type = ADOX.DataTypeEnum.adInteger   // Indicates a four byte signed integer.
      };
      idCol.Properties["AutoIncrement"].Value = true;     // Enable the auto increment property for this column.

      // Column 2 (Name)
      ADOX.Column nameCol = new ADOX.Column();
      nameCol.Name = "Name";    // The name of the column
      nameCol.ParentCatalog = catalog;
      nameCol.Type = ADOX.DataTypeEnum.adVarWChar;   // Indicates a string value type.
      nameCol.DefinedSize = 60;   // 60 characters max.


      // Column 3 (Surname)
      ADOX.Column surnameCol = new ADOX.Column();
      surnameCol.Name = "Surname";    // The name of the column
      surnameCol.ParentCatalog = catalog;
      surnameCol.Type = ADOX.DataTypeEnum.adVarWChar;   // Indicates a string value type.
      surnameCol.DefinedSize = 60;   // 60 characters max.

      table.Columns.Append(idCol);        // Add the Id column to the table.
      table.Columns.Append(nameCol);      // Add the Name column to the table.
      table.Columns.Append(surnameCol);   // Add the Surname column to the table.

      catalog.Tables.Append(table);   // Add the table to our database.             

      // Close the connection to the database after we are done creating it and adding the table to it.
      // catalog.ActiveConnection = null;
      var con = catalog.ActiveConnection;
      if (con != null && con.State != 0)
        con.Close();
      catalog = null;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      try
      {
        string connectionString = string.Format("Provider={0}; Data Source={1}; Jet OLEDB:Engine Type={2}",
       "Microsoft.Jet.OLEDB.4.0",
       "mydb.mdb",
       5);

        try
        {
          File.Delete("mydb.mdb");
        }
        catch { }
        ADOX.Catalog catalog = new ADOX.Catalog();
        catalog.Create(connectionString);



        string[] exempt = { "Client Record State", "Server Record State", "Create Timestamp", "Update Timestamp" };

        var str = @"C:\Users\Eliud\Documents\KfaCashSales\data.mdb";
        using (var con = new SQLiteConnection(string.Format(@"Data Source={0};Version=3;", str)))
        {
          con.Open();
          using (var cmdX = con.CreateCommand())
          {
            cmdX.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'tombstone_%'";
            using (var mReader = cmdX.ExecuteReader())
            {
              while (mReader.Read())
              {
                var tableName = mReader.GetValue(0).ToString().Replace("tbl_", "").Replace("sys_", "").Humanize(LetterCasing.Title);
                ADOX.Table table = new ADOX.Table
                {
                  Name = tableName // Table name
                };

                using (var cmd = con.CreateCommand())
                {
                  cmd.CommandText = string.Format("PRAGMA table_info('{0}') ", mReader.GetValue(0));
                  using (var reader = cmd.ExecuteReader())
                  {
                    while (reader.Read())
                    {
                      ADOX.Column nameCol = null;
                      var name = reader.GetValue(1).ToString().Humanize(LetterCasing.Title);
                      if (exempt.Contains(name))
                        continue;

                      var type = reader.GetValue(2).ToString();
                      var nullable = Convert.ToBoolean(reader.GetValue(3));
                      var isPrimary = Convert.ToBoolean(reader.GetValue(5));

                      if (isPrimary || name.EndsWith("Id"))
                      {
                        // Column 1 (id)
                        nameCol = new ADOX.Column
                        {
                          Name = name,  // The name of the column
                          ParentCatalog = catalog,
                          Type = ADOX.DataTypeEnum.adInteger   // Indicates a four byte signed integer.
                        };

                        if (isPrimary)
                          nameCol.Properties["AutoIncrement"].Value = true;     // Enable the auto increment property for this column.
                      }
                      else if (type.StartsWith("INTEGER(1)"))
                      {
                        // Column 2 (Name)
                        nameCol = new ADOX.Column
                        {
                          Name = name,    // The name of the column
                          ParentCatalog = catalog,
                          Type = ADOX.DataTypeEnum.adBoolean,   // Indicates a string value type.
                        };

                      }
                      else if (type.StartsWith("INTEGER"))
                      {
                        // Column 2 (Name)
                        nameCol = new ADOX.Column
                        {
                          Name = name,    // The name of the column
                          ParentCatalog = catalog,
                          Type = ADOX.DataTypeEnum.adInteger,   // Indicates a string value type.
                        };

                      }
                      else if (type.StartsWith("REAL"))
                      {
                        // Column 2 (Name)
                        nameCol = new ADOX.Column
                        {
                          Name = name,    // The name of the column
                          ParentCatalog = catalog,
                          Precision = 20,
                          Type = ADOX.DataTypeEnum.adNumeric,   // Indicates a string value type.
                        };

                      }
                      else if (type.StartsWith("TEXT") && (name.Contains("Code")))
                      {
                        var length = 255;
                        var no = Regex.Matches(type, "[0-9]+");
                        if (no.Count > 0)
                        {
                          length = Convert.ToInt32(no[0].Value);
                        }
                        // Column 2 (Name)
                        nameCol = new ADOX.Column
                        {
                          Name = name,    // The name of the column
                          ParentCatalog = catalog,
                          Type = ADOX.DataTypeEnum.adVarWChar,   // Indicates a string value type.
                          DefinedSize = Math.Min(10, length)   // 60 characters max.
                        };
                      }
                      else
                      {
                        var length = 255;
                        var no = Regex.Matches(type, "[0-9]+");
                        if (no.Count > 0)
                        {
                          length = Convert.ToInt32(no[0].Value);
                        }


                        // Column 2 (Name)
                        nameCol = new ADOX.Column
                        {
                          Name = name,    // The name of the column
                          ParentCatalog = catalog,
                          Type = ADOX.DataTypeEnum.adVarWChar,   // Indicates a string value type.
                          DefinedSize = Math.Max(6, length)  // 60 characters max.
                        };
                      }
                      // if(nameCol!=null)
                      table.Columns.Append(nameCol);   // Add the Surname column to the table.
                    }

                  }
                }
                catalog.Tables.Append(table);   // Add the table to our database. 
              }
            }
            // var obj = cmd.ExecuteScalar();
          }
        }
        // Close the connection to the database after we are done creating it and adding the table to it.
        // catalog.ActiveConnection = null;
        var conn = catalog.ActiveConnection;
        if (conn != null && conn.State != 0)
          conn.Close();
        catalog = null;
        MessageBox.Show("DOne");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {

    }

    private void Make_Db_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        using (var context = new MyDbContext("projDb"))
        {
          CreateAndSeedDatabase(context);
        }
        System.Windows.MessageBox.Show("Done");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    private void CreateAndSeedDatabase(MyDbContext context)
    {
      if (context.Tables.Count() != 0)
      {
        return;
      }
      var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Entries Table.mdb");
      var data = new RawData(path);


      var tables = data.GetTables()
          .Select(table => new DatabaseTable(table.Name, table.Description, table.TableId));
      context.Tables.AddRange(tables);


      var cols = data.GetColumns()
         .Select(col => new TableColumn(col.ColumnId,
         tables.FirstOrDefault(x => x.Name == col.TableName)?.Id, col.ColumnName, col.IsUnique,
         col.Description, col.Position, col.HasDefault, col.Default, col.ColumnFlags,
         col.IsNullable, col.DataType, col.Length))
         .Where(x => x.TableId != null);
      context.Columns.AddRange(cols);

      var colTables = from tb in tables
                      from col in cols
                      where tb.Id == col.TableId
                      select new
                      {
                        col.Id,
                        col.ColumnName,
                        TableName = tb.Name
                      };


      var primaries = data.GetPrimaryKeys()
        .Select(pri => new TablePrimary(pri.PrimaryKeyId,
        colTables.FirstOrDefault(x => x.ColumnName == pri.Name && x.TableName == pri.TableName)?.Id))
         .Where(x => x.ColumnId != null);
      context.PrimaryKeys.AddRange(primaries);

      var rels = data.GetRelations()
     .Select(rel => new TableRelation(rel.ForeignKeyId,
     colTables.FirstOrDefault(x => x.ColumnName == rel.MasterColumnName && x.TableName == rel.MasterTableName)?.Id,
     colTables.FirstOrDefault(x => x.ColumnName == rel.ForeignColumnName && x.TableName == rel.ForeignTableName)?.Id))
      .Where(x => !(x.ForeignColumnId == null || x.MasterColumnId == null));
      context.Relations.AddRange(rels);


      context.SaveChanges();
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
      try
      {
        var mainPath = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "../../../..")).FullName;
        var path = Path.Combine(mainPath, "KFADynamicsAssistant.mdb"/*"Entries Table.mdb"*/);
        var data = new RawData(path);

        var preRels = data.GetRelations();

        //path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KfaDataEntries.mdb"/*"Entries Table.mdb"*/);

        //data = new RawData(path);
        var tablesWithData = data.GetTablesThatContainsData();
        preRels = new[] { preRels, data.GetRelations() }.SelectMany(x => x)
            .Distinct().ToArray();

        preRels = preRels.GroupBy(x => $"{x.ForeignTableName}=>{x.ForeignColumnName}=>{x.MasterTableName}=>{x.MasterColumnName}")
         .Select(x => x.First()).ToArray();

        var cols = preRels.Where(x => x.MasterTableName == "General Ledgers Details").ToArray();

        var dds = cols.Select(x => $"{x.ForeignTableName}={x.ForeignColumnName}={x.MasterTableName}={x.MasterColumnName}").ToArray();

        var mmv = new
        {
          Tables = data.GetTables(),
          Columns = data.GetColumns(),
          Primaries = data.GetPrimaryKeys(),
          Relations = preRels,
          TableDatas = data.GetTables().ToDictionary(y => y.Name, x =>
            {
              var table = data.GetTablesData(x.Name);
              return table.Rows.Count > 0 ? table : null;
            }),
          Data = tablesWithData
        };

        var json = JsonConvert.SerializeObject(mmv);
        File.WriteAllText(Path.Combine(mainPath, "Entries Table.json"), json);
        System.Windows.MessageBox.Show("Done");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }
  }
}




