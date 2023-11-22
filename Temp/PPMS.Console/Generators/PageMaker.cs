using Humanizer;
using Microsoft.EntityFrameworkCore;
using PPMS.Console.Data;
using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PPMS.Console.Generators
{
    class PageMaker
    {
        public static void GenerateEntryPagesXamls()
        {
            var paths = Path.Combine(Defaults.MainPath, "MainMenus");
            if (!Directory.Exists(paths))
                Directory.CreateDirectory(paths);

            using var db = new Data.Context();
            var tbls = db.Tables.ToArray();
            var groups = Regex.Split(Groups.names, "'~\r\n'")
   .Select(x => Regex.Split(x, "'~'")).ToArray()
   .Select(x =>
   {
       var tbl = tbls.FirstOrDefault(y => y.Name == x[1] || y.Name == x[2])?.Id;
       return new DataGroup
       {
           Id = x[0],
           GroupName = x[4],
           StrimLinedName = x[2],
           ImagePath = x[3],
           TableId = tbl
       };
   }).ToArray();


            var dataObjs = new Dictionary<string, string>() { { "General", "" } };
            foreach (var item in db.Tables)
            {
                var group = groups.FirstOrDefault(x =>
                x.StrimLinedName.ToUpper().Replace(" ", "")
                == item.OriginalName.ToUpper().Replace(" ", ""));
                if (group == null)
                {
                    dataObjs["General"] += $@" #region {item.StrimLinedName}
            cmd = new CommandDetailQueryModel
            {{
                CommandName = ""{item.StrimLinedName}"",
                ImagePath = ""im-user-busy.png"",
                IsEnabled = true,
                Category = ""System""
            }};
            cmd.ToViewObject(MainMenuType.General,
                () => ControlInitializer.CheckForLoading(ViewPages.{item.StrimLinedName.MakeName()}ViewPage.Page)
                , register, ""{item.StrimLinedName} Entries Page"");
            #endregion


";
                }
                else
                {
                    var image = group.ImagePath.Replace(@"E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\", "");

                    var grp = group.GroupName.Replace("\\", "/").Split('/').FirstOrDefault() ?? "General";
                    if (!dataObjs.ContainsKey(grp))
                        dataObjs.Add(grp, "");
                    var pathDis = group.GroupName?.Substring(grp.Length);
                    dataObjs[grp] += $@" #region {item.StrimLinedName}
            cmd = new CommandDetailQueryModel
            {{
                CommandName = ""{item.StrimLinedName}"",
                ImagePath = ""{image}"",
                IsEnabled = true,
                Category = ""{pathDis}""
            }};
            cmd.ToViewObject(MainMenuType.{grp.MakeName()},
                () => ControlInitializer.CheckForLoading(ViewPages.{item.StrimLinedName.MakeName()}ViewPage.Page)
                , register, ""{item.StrimLinedName} Entries Page"");
            #endregion


";
                }
            }

            foreach (var file in dataObjs)
            {
                var body = $@"using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Views;
using Pilgrims.Projects.Assistant.Data;
using Pilgrims.Projects.Assistant.DataEntry.Classes;

namespace Pilgrims.Projects.Assistant.DataEntry.Menus
{{
    static class {file.Key.MakeName()}
    {{
        static CommandDetailQueryModel cmd;
        public static void Create(IRegisterViewObject register)
        {{
{file.Value}
        }}
    }}
}}
";
                File.WriteAllText(Path.Combine(paths, $"{file.Key.MakeName()}.cs"), body);
            }
            var rels = db.Relations
                .Include(x => x.MasterColumn.Table.Columns)
                .Include(x => x.ForeignColumn.Table.Columns)
                .Where(x => x.MasterColumnId != null && x.ForeignColumnId != null)
                .ToArray();

            //            db.Groups.Include(x => x.Tables).ToArray()
            //                .GroupBy(x =>
            //                x.GroupName.Replace("\\", "/").Split('/').FirstOrDefault() ?? "General")
            //                .Where(x => x.Key != "General")
            //                .Select(x =>
            //                {
            //                    {
            //                        var body = x.Select(m => $@"            #region {m.Tables?.Name ?? m.StrimLinedName}
            //            cmd = new {m.Tables?.Name.Singularize().MakeName()}QueryModel
            //            {{
            //                CommandName = ""{m.Tables?.Name ?? m.StrimLinedName}"",ImagePath = ""{m.ImagePath.Replace(@"E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\", "")}"",
            //                IsEnabled = true,
            //                Category = ""{(m.Tables?.Name ?? m.StrimLinedName).Substring(x.Key.Length)}""
            //            }};
            //            cmd.ToViewObject(MainMenuType.{x.Key.MakeName()},
            //                () => ControlInitializer.CheckForLoading(ViewPages.{(m.Tables?.Name ?? m.StrimLinedName).MakeName()}ViewPage.Page)
            //                , register, ""{m.Tables?.Name ?? m.StrimLinedName} Entries Page"");
            //            #endregion");

            //                        return new { name = x.Key, body = $@"using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
            //using Pilgrims.Projects.Assistant.Contracts.Views;
            //using Pilgrims.Projects.Assistant.Data;
            //using Pilgrims.Projects.Assistant.DataEntry.Classes;

            //namespace Pilgrims.Projects.Assistant.DataEntry.Menus
            //{{
            //    static class {x.Key.MakeName()}
            //    {{
            //        static CommandDetailQueryModel cmd;
            //        public static void Create(IRegisterViewObject register)
            //        {{
            //{string.Join("\r\n\r\n", body)}
            //        }}
            //    }}
            //}}
            //" };
            //                    }
            //                }).ToList().ForEach(x =>
            //                {
            //                    File.WriteAllText(Path.Combine(paths, $"{x.name}.cs"), x.body);
            //                });

            var tt = rels.Where(x => x.ForeignColumn == null || x.MasterColumn == null).ToArray();

            var allRels = rels.ToArray();

            QueryMaker.CreateModelQuery(db.Tables
                .Include(x => x.Columns), allRels);

            var addPageResolver = AvaloniaTemplates.GetAddPageResolver(db.Tables.ToArray());
            var enumTypeConverter = AvaloniaTemplates.GetEnumTypeConverter(db.Tables.ToArray());
            var autoCompletionFile = AvaloniaTemplates.GetAutoCompleteFile(db.Tables.ToArray(), db.Relations.Include(v => v.ForeignColumn).Include(v => v.MasterColumn).ToArray());

            if (!Directory.Exists(Path.Combine(Defaults.MainPath, "General")))
                Directory.CreateDirectory(Path.Combine(Defaults.MainPath, "General"));
            File.WriteAllText(Path.Combine(Defaults.MainPath, "General", "AddPageResolver.cs"),
                addPageResolver);
            File.WriteAllText(Path.Combine(Defaults.MainPath, "General", "EnumTypeConverter.cs"),
              enumTypeConverter);
            File.WriteAllText(Path.Combine(Defaults.MainPath, "General", "AutoCompletionData.cs"),
            autoCompletionFile);

            var data = db.Tables
                .Include(x => x.Columns)
                .Select(x => new { x.Name, Table = x, x.Columns })
                .ToArray()
                .Select(x =>
                {
                    var ctrls = new Dictionary<TableColumn, string>();
                    var sbRows = new StringBuilder();
                    var sbCols = new StringBuilder();
                    int i = 0;
                    var cols = x.Columns.OrderByDescending(y => y.IsPrimary)
                    .ThenBy(y => ViewMaker.selfCalculated.Any(m => y.ColumnName.ToLower()
                    .Contains(m.ToLower())))
                    .ThenBy(y => y.ColumnName.ToLower().Contains("narration"))
                    .ThenBy(y => y.DataType == 6)
                    .ThenBy(y => y.ColumnName.ToLower().Contains("no of") || y.ColumnName.ToLower().Contains("number of"))
                    .ThenBy(y => y.Type.ToLower().Contains("date"))
                    .ThenBy(y => y.Position).ToArray();

                    int captionRow = cols.Length + 1;
                    foreach (var col in cols)
                    {
                        sbRows.Append(@" <RowDefinition Height=""Auto"" MinHeight=""40"" />
");
                        var txt = ViewMaker.GetControlText(col, i++, allRels, captionRow, out bool hasCaption);
                        if (hasCaption)
                            captionRow++;

                        ctrls.Add(col, txt[1]);
                        sbCols.Append(txt[0]).AppendLine().AppendLine();
                    }

                    var currentRels = rels
                    .Where(y => y.ForeignColumn.Table?.Name == x.Name ||
                    y.MasterColumn.Table.Name == x.Name);

                    return new
                    {
                        TableName = x.Name,
                        Name = $"{x.Name.MakeName()}EntryPage",
                        VMName = $"{x.Name.MakeName()}ViewModel",
                        Rows = sbRows.ToString(),
                        x.Table,
                        Controls = sbCols.ToString(),
                        ViewPage = Templates.GetViewXamlPage(x.Table, currentRels.ToArray(), cols),
                        AvaloniaAddXamlPage = AvaloniaTemplates.GetViewXamlPage(x.Table, currentRels.ToArray(), cols),
                        AvaloniaAddBehindPage = AvaloniaTemplates.GetViewPageCodeBehind(x.Table, currentRels.ToArray(), cols),
                        AvaloniaViewModelPage = AvaloniaTemplates.GetPageViewModel(x.Table, currentRels.ToArray(), cols),
                        ViewPageViewModel = Templates.GetPageViewModel(x.Table, currentRels.ToArray(), cols),
                        ViewBehindPages = Templates.GetViewPageCodeBehind(x.Table, currentRels.ToArray(), cols),
                        ViewBehindPage = AvaloniaTemplates.GetForViewPageCodeBehind(x.Table, currentRels.ToArray(), cols),
                        BehindCode = ViewMaker.GetBehindCode(x.Name, ctrls),
                        ViewModel = ViewMaker.GetViewModel(x.Table, allRels)
                    };
                }).ToArray();

            var path1 = Path.Combine(Defaults.MainPath, "EntryPages");
            if (!Directory.Exists(path1))
                Directory.CreateDirectory(path1);

            var path2 = Path.Combine(Defaults.MainPath, "Avalonia", "AddPages");
            if (!Directory.Exists(path2))
                Directory.CreateDirectory(path2);

            var path3 = Path.Combine(Defaults.MainPath, "Avalonia", "ViewModels");
            if (!Directory.Exists(path3))
                Directory.CreateDirectory(path3);

            var path = Path.Combine(Defaults.MainPath, "EntryViewPages");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.Exists(Path.Combine(path, "ViewModels")))
                Directory.CreateDirectory(Path.Combine(path, "ViewModels"));

            data.Select(x => new
            {
                Text =
                ViewMaker.GetEntryPageText(x.Name, x.Rows, x.Controls),
                x.Name,
                x.ViewPage,
                x.Table,
                x.ViewBehindPage,
                x.ViewPageViewModel,
                x.BehindCode,
                x.AvaloniaAddBehindPage,
                x.AvaloniaViewModelPage,
                x.AvaloniaAddXamlPage
            })
                .ToList().ForEach(text =>
                {
                    File.WriteAllText(Path.Combine(path, $"{text.Table.Name.MakeName()}ViewPage.xaml"), text.ViewPage);
                    File.WriteAllText(Path.Combine(path, $"{text.Table.Name.MakeName()}ViewPage.xaml.cs"), text.ViewBehindPage);
                    File.WriteAllText(Path.Combine(Path.Combine(path, "ViewModels"), $"{text.Name}ViewModel.cs".Replace("EntryPage", "")), text.ViewPageViewModel);
                    File.WriteAllText(Path.Combine(path1, $"{text.Name}.xaml"), text.Text);
                    File.WriteAllText(Path.Combine(path1, $"{text.Name}.xaml.cs"), text.BehindCode);

                    File.WriteAllText(Path.Combine(path2, $"{text.Name}.xaml".Replace("EntryPage", "AddPage")), text.AvaloniaAddXamlPage);
                    File.WriteAllText(Path.Combine(path2, $"{text.Name}.xaml.cs".Replace("EntryPage", "AddPage")), text.AvaloniaAddBehindPage);
                    File.WriteAllText(Path.Combine(path3, $"{text.Name}ViewModel.cs".Replace("EntryPage", "")), text.AvaloniaViewModelPage);
                });

            path = Path.Combine(Defaults.MainPath, "ViewModels");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            data.Select(x => new
            {
                Name = x.VMName,
                x.ViewModel
            }).ToList().ForEach(text => File.WriteAllText(Path.Combine(path, $"{text.Name}.cs"), text.ViewModel));

            path = Path.Combine(Defaults.MainPath, "Pages");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            data.Select(x => new
            {
                Page = ViewMaker.GetPage(x.Table, allRels),
                Name = x.Table.Name.MakeName()
            }).ToList().ForEach(text =>
            {
                File.WriteAllText(Path.Combine(path, $"{text.Name}page.xaml"), text.Page[0]);
                File.WriteAllText(Path.Combine(path, $"{text.Name}page.xaml.cs"), text.Page[1]);
            });
        }
    }
}
