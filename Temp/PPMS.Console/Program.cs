using System.Linq;
using static System.Console;
using System;
using System.IO;
using System.Threading.Tasks;
using PPMS.Console.Generators;
using PPMS.Console.Models;
using PPMS.Console.Data;

namespace PPMS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var nn = Groups.GetTables();
            //CheckCurrentDb.CheckData();
            DataObject.CheckToReset();
            using var db = new Data.Context();
            ProjectSelectors.Generate();

            // using var tsk2h2 = Task.Factory.StartNew (PPMS.Console.Generators.Controllers.GenerateDtos);
            // Task.WaitAll (tsk2h2);

            Repository.Generate();

            //var nn = db.Tables?.Count ().ToString ();
            // System.Console.WriteLine (" == > " + nn);
            // foreach (var item in Data.OldData.DefaultTables)
            // {
            //   //  System.Console.WriteLine (string.Format("{0} => {1}={2}", item.Name, item.OriginalName, item.Id));
            //   db.Tables.Add(item);
            // }

            // foreach (var item in db.Columns)
            //    db.Columns.Remove(item);

            // foreach (var item in Data.OldData.DefaultColumns)
            // {
            //   var col = Data.OldData.DefaultTables.FirstOrDefault(x => x.Name.MakeName() == item.TableId.MakeName());
            //   item.TableId = col?.Id;
            //   db.Columns.Add(item);
            // }
            // foreach (var item in db.Relations)
            //   db.Relations.Remove(item);

            // foreach (var item in Data.OldData.DefaultRelations)
            // {
            //   db.Relations.Add(item);
            //   db.SaveChanges();
            // }
            // foreach (var item in db.PrimaryKeys)
            //   db.PrimaryKeys.Remove(item);

            // foreach (var item in Data.OldData.DefaultPrimaryKeys)
            // {
            //   if (!string.IsNullOrWhiteSpace(item.ColumnId))
            //   {
            //     db.PrimaryKeys.Add(item);
            //     db.SaveChanges();
            //   }
            // }

            // foreach (var item in db.Groups)
            //   db.Groups.Remove(item);

            // foreach (var item in Data.OldData.DefaultGroups)
            // {
            //   if (!string.IsNullOrWhiteSpace(item.TableId))
            //   {
            //     db.Groups.Add(item);
            //     db.SaveChanges();
            //   }
            // }

            //  foreach (var item in db.InitialData)
            //   db.InitialData.Remove(item);

            // foreach (var item in Data.OldData.DefaultData)
            // {
            //   if (!string.IsNullOrWhiteSpace(item.TableId))
            //   {
            //     db.InitialData.Add(item);
            //     db.SaveChanges();
            //   }
            // }
            //PPMS.Console.Generators.Controllers.Generate();
            SeedData.GenerateContexts();
            using var tsk22 = Task.Factory.StartNew(Controllers.GenerateDtos);
            using var tsk = Task.Factory.StartNew(EfDbContext.Generate);
            using var tsk2 = Task.Factory.StartNew(Repository.Generate);
            using var tsk3 = Task.Factory.StartNew(Controllers.Generate);
            using var tsk4 = Task.Factory.StartNew(GraphQlGenerators.Generate);
            using var tsk5 = Task.Factory.StartNew(PageMaker.GenerateEntryPagesXamls);
            using var tsk6 = MagicOnionClasses.Generate();
            Task.WaitAll(tsk, tsk2, tsk3, tsk22, tsk4, tsk5, tsk6);
            foreach (var obj in new[] { tsk, tsk2, tsk22, tsk3, tsk4, tsk5, tsk6 })
                obj.Dispose();

            WriteLine("Done");

            //SELECT 
            //    m.name
            //    , p.*
            //FROM
            //    sqlite_master m
            //    JOIN pragma_foreign_key_list(m.name) p ON m.name != p."table"
            //WHERE m.type = 'table'
            //ORDER BY m.name
            //;

            WriteLine("Enter list of number separated with space Ex: {1 2 3 4 5}:");
            var numbers = ReadLine().Trim().Split(' ').Select(int.Parse).ToArray();
            var currentValue = 0;
            var totalSwaps = 0;

            for (int i = 0; i <numbers.Length; i++)
            {
                int numberOfSwaps = 0;

                for (int j = 0; j <numbers.Length - 1; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        currentValue = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = currentValue;
                        numberOfSwaps++;
                    }
                }

                totalSwaps += numberOfSwaps;
                if (numberOfSwaps == 0)
                {
                    break;
                }
            }

            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var logPath = Path.Combine(docPath, "Tester.txt");
            using (var writer = File.CreateText(logPath))
            {
                writer.WriteLine("log message"); //or .Write(), if you wish
            }
            WriteLine(logPath);

            WriteLine($"Array is sorted in {totalSwaps} swaps.");
            WriteLine($"First Element: {numbers.First()}");
            WriteLine($"Last Element: {numbers.Last()}");

            WriteLine("\n\nPress any key ...");
            ReadKey();
        }
    }
}