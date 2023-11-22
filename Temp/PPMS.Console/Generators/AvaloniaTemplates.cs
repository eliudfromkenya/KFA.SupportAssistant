using Humanizer;
using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PPMS.Console.Generators
{
    static class AvaloniaTemplates
    {
        public static string GetViewXamlPage(DatabaseTable table, TableRelation[] rels, TableColumn[] cols)
        {
            try
            {
                int i = 0;
                var captionRow = 15;
                var colvs = string.Join("\r\n\r\n", cols.Select(col =>
                {
                    var ff = ViewMaker.GetControlText(col, i, rels, captionRow, out bool hasCaption, 2);
                    i += 2;
                    if (hasCaption)
                        captionRow++;
                    return ff[0].Replace("Classes=\"caption\"", "Classes=\"caption smallWidth\"");
                }));


                var gridRows = string.Join(",", Enumerable.Repeat("Auto", captionRow + 3));
                var name = table.Name.MakeName();
                return $@"<UserControl xmlns=""https://github.com/avaloniaui""
                xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
        xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
        xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        mc:Ignorable=""d"" d:DesignWidth=""800"" d:DesignHeight=""450""
        xmlns:i=""clr-namespace:Avalonia.Xaml.Interactivity;assembly=Avalonia.Xaml.Interactivity""
        xmlns:behaviors=""clr-namespace:Pilgrims.Projects.Assistant.Contracts.Behaviors;assembly=Pilgrims.Projects.Assistant.Contracts""
        x:Class=""Pilgrims.Projects.Assistant.DataEntry.AddPages.{name}AddPage""
        MaxWidth=""1100""
        xmlns:buttons=""clr-namespace:Pilgrims.Projects.Assistant.Contracts.Controls;assembly=Pilgrims.Projects.Assistant.Contracts"" >
  <Grid ColumnDefinitions=""10,Auto,*,Auto,*,10"" RowDefinitions=""Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,*,Auto,Auto,Auto,Auto,Auto,Auto,20"" VerticalAlignment=""Stretch"" HorizontalAlignment=""Stretch"" >       
           <Border BorderBrush=""Gray"" BorderThickness=""3"" CornerRadius=""10""  Grid.ColumnSpan=""200"" Grid.RowSpan=""200"" />  
               <TextBlock Classes=""secondary h2"" Text=""{table.Name.Singularize()} Details"" Margin=""40"" HorizontalAlignment=""Center"" Grid.ColumnSpan=""4"" Grid.Column=""1"" />                           
                 {colvs}
         <buttons:BatchControls Name=""Buttons"" Grid.ColumnSpan=""10"" Grid.Row=""18"" />   
         <TextBlock Name=""MessageBlock""  TextWrapping=""Wrap"" Grid.ColumnSpan=""10"" Grid.Row=""20"" >
           <i:Interaction.Behaviors>
              <behaviors:TextBlockMessageBehavior />
           </i:Interaction.Behaviors>
         </TextBlock>
       </Grid >
     </UserControl >

     ".Replace("Pilgrims.Projects.Assistant", "Pilgrims.Projects.Assistant");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string GetAddPageResolver(DatabaseTable[] tables)
        {
            return $@"using Avalonia.Controls;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.DataEntry.AddPages;
using System;

namespace Pilgrims.Projects.Assistant.DataEntry
{{{{
    static class AddPageResolver
    {{
        public static Window GetPage<T>(Action<T> getId)
        {{
            return GetPage(typeof(T), getId);
        }}

        public static Window GetPage(Type type, object actionObject)
        {{
{string.Join("\r\n", tables.Select(c => $@"            if (type == typeof({Functions.Singularize(c.Name).MakeName()}QueryModel))
                return new {c.Name.MakeName()}AddPage(actionObject as Action<{Functions.Singularize(c.Name).MakeName()}QueryModel>);"))}
            throw new Exception(""Unable to resolve add page for "" + type.Name);
        }}

        public static Window GetPage(TablesEnum tableEnum, object getId)
        {{
            return tableEnum switch
            {{
{string.Join("\r\n", tables.Select(c => $@"TablesEnum.{c.Name.MakeName()} => GetPage(typeof({Functions.Singularize(c.Name).MakeName()}QueryModel), getId),"))}                
                _ => throw new Exception($""Unable to resolve add page for {{tableEnum}}""),
            }};
        }}
    }}
}}
";
        }



        public static string GetEnumTypeConverter(DatabaseTable[] tables)
        {
            return $@"using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using System;

namespace Pilgrims.Projects.Assistant.Contracts.Classes
{{
    public static class EnumTypeConverter
    {{
        public static TablesEnum GetEnumFromType<T>()
        {{
            return GetEnumFromType(typeof(T));
        }}

        public static TablesEnum GetEnumFromType(Type type)
        {{
{string.Join("\r\n", tables.Select(c => $@"            if (type == typeof({Functions.Singularize(c.Name).MakeName()}QueryModel))
                return TablesEnum.{c.Name.MakeName()};"))}
            throw new Exception($""Unable to convert enum value of model {{ type.Name }}"");
        }}


        public static Type GetPage(TablesEnum tableEnum)
        {{
            return tableEnum switch
            {{
{string.Join("\r\n", tables.Select(c => $@"TablesEnum.{c.Name.MakeName()} => typeof({Functions.Singularize(c.Name).MakeName()}QueryModel),"))}                
                _ => throw new Exception($""Unable to resolve model for '{{tableEnum}}'""),
            }};
        }}
    }}
}}
";
        }

        public static string GetPageViewModel(DatabaseTable table, TableRelation[] rels, TableColumn[] cols)
        {

            var relIds = rels.Select(x => x.ForeignColumnId).ToList();
            relIds.AddRange(rels.Select(x => x.MasterColumnId));
            relIds = relIds.Distinct().ToList();

            string getValidation()
            {
                var sb = new List<String>();
                foreach (var col in cols)
                {
                    if (col.IsNullable || col.IsPrimary || col.Type.ToLower().Contains("bool"))
                    {
                        continue;
                    }

                    if (col.Type.ToLower().Contains("string") || relIds.Contains(col.Id))
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || !string.IsNullOrWhiteSpace(text),
                            ""Please {1} is required, can't be empty"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }
                    else if (col.Type.ToLower().Contains("date"))
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    date => CurrentObject == null || date > new DateTime(1900,1,1),
                            ""Please select a valid {1} date..."");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }
                    else
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    num => CurrentObject == null || num > 0,
                            ""Please {1} is required, can't be empty"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }
                }

                foreach (var col in cols)
                {
                    var reg = Regex.Match(col.ColumnName, "item.*(id|Code|Key|Number|No)$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckItemCode(text),
                            ""Please check the item code is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    reg = Regex.Match(col.ColumnName, "(branch|cost *centre).*(id|Code|Key|Number|No)$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckBranchCode(text),
                            ""Please check cost centre code is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    reg = Regex.Match(col.ColumnName, "supplier.*(id|Code|Key|Number|No)$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckSupplierCode(text),
                            ""Please the supplier code is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    reg = Regex.Match(col.ColumnName, "ledger.*(id|Code|Key|Number|No)$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckLedgerCode(text),
                            ""Please ledger code is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    reg = Regex.Match(col.ColumnName, "cheque.*(id|Code|Key|Number|No)$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckChequeNumber(text),
                            ""Please check cheque number is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    reg = Regex.Match(col.ColumnName, "month$", RegexOptions.IgnoreCase);
                    if (reg.Success && col.Type.ToLower().Contains("string"))
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckMonth(text),
                            ""Please month is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    reg = Regex.Match(col.ColumnName, "email", RegexOptions.IgnoreCase);
                    if (reg.Success)
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckEmail(text),
                            ""Please email is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    reg = Regex.Match(col.ColumnName, "(phone|contact|mobile)", RegexOptions.IgnoreCase);
                    if (reg.Success)
                    {
                        sb.Add(string.Format(@" this.ValidationRule(
                    viewModel => viewModel.CurrentObject.{0},
                    text => CurrentObject == null || DataEntryValidations.CheckPhoneNumber(text),
                            ""Please phone number is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower()));
                    }

                    //reg = Regex.Match(col.Type, "([^string]|[^date]|[^bool]|[^time])$", RegexOptions.IgnoreCase);
                    //if (reg.Success)
                    //{
                    //    sb.Add(string.Format(@" this.ValidationRule(
                    //viewModel => viewModel.CurrentObject.{0},
                    //text => CurrentObject == null || DataEntryValidations.CheckItemCode(text),
                    //        ""Please your item code is not valid"");", col.ColumnName.Titleize().MakeName(), col.ColumnName.ToLower());
                    //}
                }
                return $"            {string.Join("\r\n\r\n            ", sb.Distinct())}";
            }
            // var gridRows = string.Join(",", Enumerable.Repeat("Auto", captionRow + 3));
            var name = table.Name.MakeName();
            var singular = Functions.Singularize(name);
            var singularName = Functions.Singularize(name).Camelize();

            return $@"
using ReactiveUI.Validation.Extensions;
using Pilgrims.Projects.Assistant.Contracts.ViewModels;
using Pilgrims.Projects.Assistant.Contracts.DataLayer.Models;

namespace Pilgrims.Projects.Assistant.DataEntryViewModels
{{
    public class {name}ViewModel : DataEntryCommandsViewModel<I{singular}Model>
    {{
        public override string Category {{ get; protected set; }} = ""{table.Name}"";

        public {name}ViewModel()
        {{           
            AddValidationRules();
        }}

        private void AddValidationRules()
        {{
{getValidation()}
        }}       
    }}
}}

";
        }

        public static string GetViewPageCodeBehind(DatabaseTable table, TableRelation[] rels, TableColumn[] cols)
        {
            int i = 0;
            var captionRow = cols.Length * 2 + 1;
            var relIds = rels.SelectMany(x => new[] { x.ForeignColumnId, x.MasterColumnId }).ToArray();

            static string GetControlSelectionProperty(TableColumn tt)
            {
                if (tt.Type.ToLower().Contains("date"))
                {
                    return "Text";
                }
                else if (tt.Type.ToLower().Contains("bool"))
                {
                    return "IsChecked";
                }
                else if (tt.Type.ToLower().Contains("dates"))
                {
                    return "SelectedDate";
                }
                else
                {
                    return "Text";
                }
            }


            static string GetControlTypeProperty(TableColumn tt, string[] rels)
            {
                if (tt.IsPrimary)
                    return "TextBox";

                if (tt.Type.ToLower().Contains("date"))
                {
                    return "AutoCompleteBox";
                }
                else if (tt.Type.ToLower().Contains("bool"))
                {
                    return "CheckBox";
                }
                else if (tt.Type.ToLower().Contains("string") || rels.Any())
                {
                    return "AutoCompleteBox";
                }
                else
                {
                    return "TextBox";
                }
            }

            var captionLabelsBindings = new List<string>();
            var checkMethods = new List<string>();
            var colvs = cols.Select(col =>
            {
                var ff = ViewMaker.GetControlText(col, i += 2, rels, captionRow, out bool hasCaption);
                // if (col.DataType == 6)
                // {
                //     checkMethods.Add($"\r\n            CheckAmount({ff[1].Titleize().MakeName()}, disposal);");
                // }
                // if ((col.ColumnName?.ToLower().Contains("month") ?? false) &&
                //!Regex.IsMatch(col.ColumnName, "[0-9]", RegexOptions.IgnoreCase))
                // {
                //     checkMethods.Add($"\r\n            CheckMonth({ff[1].Titleize().MakeName()}, disposal);");
                // }
                if (hasCaption)
                    if (hasCaption)
                    {
                        captionRow++;
                        if (ff.Length == 3)
                        {
                            captionLabelsBindings.Add($@"
            var {ff[2].Camelize()} = this.FindControl<TextBlock>(""{ff[2]}"");
            InitializeCaptionLabel({ff[2].Camelize()}, disposal, false);
            if ({ff[2].Camelize()} != null)
            {{
                Context.CurrentObject.WhenAnyValue(c => c.{col.ColumnName.MakeName()})
                   .Throttle(TimeSpan.FromMilliseconds(700))
                   .DistinctUntilChanged()
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(c =>
                   {{
                       var text = CurrencyToWordsConverter.ConvertToWords(c);
                       if (!string.IsNullOrWhiteSpace(text))
                          {ff[2].Camelize()}.Text = text;
                   }}).DisposeWith(disposal);
            }}
                            ");
                        }
                        else if (ff.Length == 4)
                        {
                            captionLabelsBindings.Add($@"
            var {ff[2].Camelize()} = this.FindControl<TextBlock>(""{ff[2]}"");
            InitializeCaptionLabel({ff[2].Camelize()}, disposal, true);
            if ({ff[2].Camelize()} != null)
            {{
                Context.CurrentObject.WhenAnyValue(c => c.{col.ColumnName.MakeName()})
                   .Throttle(TimeSpan.FromMilliseconds(700))
                   .DistinctUntilChanged()
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(c =>
                   {{
                       var pairs = AutoCompletionData.{ff[3]}.Where(b => b.Key == c);
                       if (pairs.Any())
                          {ff[2].Camelize()}.Text = pairs.First().Value;
                   }}).DisposeWith(disposal);
            }}
                            ");
                        }
                    }
                var ctrName = ff[1].Titleize().MakeName();

                var foreigns = rels
                .Where(x => cols.Any(n => n.Id == x.ForeignColumnId))
                .Select(x => x.MasterColumnId).ToArray();

                var masters = rels
                              .Where(x => cols.Any(n => n.Id == x.ForeignColumnId))
                              .Select(x => x.ForeignColumnId).ToArray();

                var key = $@"        public {(ctrName.ToLower().StartsWith("txl") ? "TextBlock" : GetControlTypeProperty(col, foreigns))} {ctrName} {{get;}}
        public TextBlock {col.ColumnName.Titleize().MakeName()}ErrorBox  {{get;}}";

                var needsConverter = new[] { "AutoCompleteBox", "TextBox" }.Contains(GetControlTypeProperty(col, foreigns));
                string binding;
                var relIds = rels.SelectMany(x => new[] { x.ForeignColumnId, x.MasterColumnId }).Distinct().ToArray();
                if (!relIds.Contains(col.Id) && needsConverter && col.Type?.ToLower() != "string" && !col.IsPrimary)
                {
                    binding = $@"            this.Bind(ViewModel,
           viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
           view => view.{ctrName}.{GetControlSelectionProperty(col)},
           ValueConverters.From{col.Type.Titleize().MakeName()}, ValueConverters.To{col.Type.Titleize().MakeName()}).DisposeWith(disposal);";
                }
                else
                {
                    binding = $@"            this.Bind(ViewModel,
           viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
           view => view.{ctrName}.{GetControlSelectionProperty(col)}).DisposeWith(disposal);";
                }
                //     var controlAssignment = $@"            this.Bind(ViewModel,
                //viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
                //view => view.{ctrName}.{GetControlSelectionProperty(col)}).DisposeWith(disposables);";

                var ctrl = $@"            {ctrName} = this.FindControl<{(ctrName.ToLower().StartsWith("txl") ? "TextBlock" : GetControlTypeProperty(col, foreigns))}>(""{ff[1]}"");
            {col.ColumnName.Titleize().MakeName()}ErrorBox = this.FindControl<TextBlock>(""{col.ColumnName.Titleize().MakeName()}ErrorBox"");";


                string autoCompleteText = "";
                string autoCompleteAssignment = "";
                if (!col.IsPrimary && ((col.Type?.ToLower().Contains("string") ?? false) || relIds.Contains(col.Id)))
                {
                    autoCompleteText = $@"                    if (Context.AutoCompleteList.ContainsKey(nameof(k.{col.ColumnName.MakeName()})))
                        this.OneWayBind(ViewModel,
                           viewModel => viewModel.AutoCompleteList[nameof(k.{col.ColumnName.MakeName()})],
                           view => view.{ctrName}.Items).DisposeWith(disposal);";
                    var foreignTable = rels
                          .Where(x => x.ForeignColumnId == col.Id)
                          .Select(x => x.MasterColumn.Table.StrimLinedName.MakeName())
                          .FirstOrDefault();

                    if (foreignTable?.Length > 2)
                        autoCompleteAssignment = $@"                {{ nameof(k.{col.ColumnName.MakeName()}), AutoCompletionData.{foreignTable.MakeName()} }}";
                    else
                        autoCompleteAssignment = $@"                {{
                    nameof(k.{col.ColumnName.MakeName()}),
                    Context.ModelList.Select(c => c.{col.ColumnName.MakeName()}).Distinct()
                           .Select(c => new Pair(c)).ToArray()
                }}";
                }

                var errorBindings = $@"                {{
                    {ctrName}, disposable =>
                       this.BindValidation(ViewModel,
                           viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.MakeName())},
                           view => view.{col.ColumnName.Titleize().MakeName()}ErrorBox.Text)
                              .DisposeWith(disposable)
                }}";

                var contextControls = $@"                new({ctrName}, {col.ColumnName.Titleize().MakeName()}ErrorBox, ""{col.ColumnName}"", typeof({(relIds.Contains(col.Id) ? "string" : col.Type)}))";

                return new
                {
                    //TableColumn = col,
                    Key = key,
                    Binding = binding,
                    ErrorBinding = errorBindings,
                    Control = ctrl,
                    //Auto = res,
                    //ValidationLabel = validationError,
                    //Ctrl = controlAssignment,
                    AutoCompleteAssignment = autoCompleteAssignment,
                    AutoCompleteText = autoCompleteText,
                    ContextControl = contextControls
                };
            });

            if (checkMethods.Count > 4)
                checkMethods.Insert(0, "\r\n");

            var name = table.Name.MakeName();
            var singular = Functions.Singularize(name);
            var sb = new StringBuilder($@"

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pilgrims.Projects.Assistant.Contracts.Controls;
using Pilgrims.Projects.Assistant.DataEntryViewModels;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using ReactiveUI;
using Pilgrims.Projects.Assistant.Contracts;
using ReactiveUI.Validation.Extensions;
using Pilgrims.Projects.Assistant.Contracts.Messaging;
using Pilgrims.Projects.Assistant.Contracts.Classes;
using System.Linq;
using System.Reactive.Linq;
using Pilgrims.Projects.Assistant.Contracts.Pages;
using Pilgrims.Projects.Assistant.Contracts.DataLayer.Models;
using Pilgrims.Projects.Assistant.Classes;

namespace Pilgrims.Projects.Assistant.DataEntry.AddPages
{{
    public class {name}AddPage : AddPageBase<{name}ViewModel, I{singular}Model>
    {{
        protected override Tk GetChildControl<Tk>(string name) => this.FindControl<Tk>(name);

        public {name}AddPage() => InitializeComponent();

        public {name}AddPage(Action<I{singular}Model> getId)
        {{
            GetObject = getId;
            InitializeComponent();
            Buttons = this.FindControl<BatchControls>(""Buttons"");
{string.Join("\r\n", colvs.Select(c => c.Control))}
        }}

        protected override BatchControls Buttons {{ get; }}
{string.Join("\r\n", colvs.Select(c => c.Key))}

        protected override List<(IControl, TextBlock, string, Type)> GetAllEntryControls()
        {{
            return new List<(IControl, TextBlock, string, Type)>
            {{
{string.Join(",\r\n", colvs.Select(c => c.ContextControl))}
            }};
        }}
 
        protected override void Loaded(CompositeDisposable disposal)
        {{
            AssignBindings(disposal);{string.Join("", checkMethods.Distinct())}
            Functions.RunOnBackground(() => AssignAutocompletions(disposal));{(captionLabelsBindings.Any() ? "\r\n            Functions.RunOnMain(() => AddCaptionLabels(disposal));" : "")}
        }}
{(captionLabelsBindings.Any() ? @"
        private void AddCaptionLabels(CompositeDisposable disposal)
        {" + string.Join("\r\n", captionLabelsBindings.Distinct()) + @"
        }
" : "")}
        private void AssignAutocompletions(CompositeDisposable disposal)
        {{  
            if (Context.AutoCompleteList == null)
                ReassignAutoCompletion();
            AutoCompletionData.HasChanged
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(x =>
                {{
                    ReassignAutoCompletion();
                }}).DisposeWith(disposal);
            Functions.RunOnMain(() =>
            {{
                try
                {{
                    var k = DataLayerFactory.CreateDataModel<I{singular}Model>();
{string.Join("\r\n", colvs.Select(c => c.AutoCompleteText).Where(b => !string.IsNullOrWhiteSpace(b)))}
                }}
                catch (Exception ex)
                {{
                    Notifier.NotifyError(""Unable to bind autocompletes"", ""Initializa Error"", ex);
                }}
            }});
        }}

        protected override void ReassignAutoCompletion()
        {{
            var k = DataLayerFactory.CreateDataModel<I{singular}Model>();
            var dic = new Dictionary<string, Pair[]>
            {{
{string.Join(",\r\n", colvs.Select(c => c.AutoCompleteAssignment).Where(b => !string.IsNullOrWhiteSpace(b)))}
            }};
            if (Context.AutoCompleteList == null)
                Context.AutoCompleteList = dic;
            else
            {{
                foreach (var item in dic)
                {{
                    if (Context.AutoCompleteList.ContainsKey(item.Key))
                        Context.AutoCompleteList[item.Key] = item.Value;
                    else Context.AutoCompleteList.Add(item.Key, item.Value);
                }}
            }}
        }}


        private void AssignBindings(CompositeDisposable disposal)
        {{
{string.Join("\r\n", colvs.Select(c => c.Binding))}
        }}

        private void InitializeComponent()
        {{
            AvaloniaXamlLoader.Load(this);
        }}
        protected override Dictionary<IControl, Action<CompositeDisposable>> AssignValidationsControls()
        {{
            return new Dictionary<IControl, Action<CompositeDisposable>>
            {{
{string.Join(",\r\n", colvs.Select(c => c.ErrorBinding))}
            }};
        }}
    }}
}}
   
            ");

            return sb.ToString();
        }


        public static string GetAutoCompleteFile(DatabaseTable[] tables, TableRelation[] rels)
        {
            var masterTables = tables
                .Where(b => rels.Any(v => v.MasterColumn.TableId == b.Id))
                .ToList();

            var sb = new StringBuilder(@"using Pilgrims.Projects.Assistant.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Classes;
using Pilgrims.Projects.Assistant.Contracts.Data.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Messaging;
using MonkeyCache.LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.Classes
{
    static class AutoCompletionData
    {
        public static BehaviorSubject<TablesEnum?> HasChanged { get; } = new BehaviorSubject<TablesEnum?>(null);
        static DateTime? LastUpdate;
");
            sb.Append(string.Join("\r\n", masterTables.Select(l => $"        public static Pair[] {l.StrimLinedName.MakeName()} => GetData(TablesEnum.{l.StrimLinedName.MakeName()});")));
            sb.Append(@"      static AutoCompletionData()
        {
            Functions.RunOnBackground(() =>
            {
                try
                {
                    AssignInitialValues();

                    Observable
                        .FromAsync(() => UpdateCache())
                        .Wait();

                    Observable.Interval(TimeSpan.FromMinutes(5))
                    .Subscribe(async c =>
                    {
                        try
                        {
                            await UpdateCache();
                        }
                        catch (Exception ex)
                        {
                            Notifier.NotifyError(""Error in querying autocomplete data"", ""Autocompletion Data"", ex);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Notifier.NotifyError(""Error in loading autocomplete data"", ""Autocompletion Data"", ex);
                }
            });
        }
   
        static void AssignInitialValues()
        {
            new List<TablesEnum> 
            {
");
            sb.Append(string.Join(",\r\n", masterTables.Select(l => $"                TablesEnum.{l.StrimLinedName.MakeName()}")));

            sb.Append(@"
            }.ForEach(x => HasChanged.OnNext(x));
        }

        static async Task UpdateCache()
        {
            var generalService = Functions.GetApiService<IGeneralService>(false);
            var data = await generalService.UpdateAutoCompletions(LastUpdate);
            LastUpdate = data.Item1;

            foreach (var obj in data.Item2)
            {
                if (obj.Item3.Any())
                {
                    var key = $""AutocompletionDataOf{(TablesEnum)obj.Item1}"";
                    // var dataObjects = .Select(c => new Pair(c.Key, c.Value)).ToArray();
                    if (!Barrel.Current.Exists(key))
                    {
                        Barrel.Current.Add(key, obj.Item3, TimeSpan.FromDays(1000));
                    }
                    else if (obj.Item2)
                    {
                        Barrel.Current.Empty(key);
                        Barrel.Current.Add(key, obj.Item3, TimeSpan.FromDays(1000));
                    }
                    else
                    {
                        var pairs = Barrel.Current
                            .Get<Dictionary<string, string>>(key) ?? new Dictionary<string, string>();

                        foreach (var item in obj.Item3)
                        {
                            if (pairs.ContainsKey(item.Key))
                            {
                                pairs[item.Key] = item.Value;
                            }
                            else
                            {
                                pairs.Add(item.Key, item.Value);
                            }
                        }
                        Barrel.Current.Empty(key);
                        Barrel.Current.Add(key, pairs, TimeSpan.FromDays(1000));
                    }
                    HasChanged.OnNext((TablesEnum)obj.Item1);
                }
            }
        }

        static Pair[] GetData(TablesEnum table)
        {
            lock (HasChanged)
            {
                try
                {
                    var key = $""AutocompletionDataOf{table}"";

                    return Barrel.Current.Get<Dictionary<string, string>>(key)
                        .Select(x => new Pair(x.Key, x.Value))
                        .OrderBy(c => c.Value)
                        .ToArray();
                }
                catch (Exception ex)
                {
                    return Array.Empty<Pair>();
                }
            }
        }
    }
}
");
            return sb.ToString();
        }

        public static string GetForViewPageCodeBehind(DatabaseTable table, TableRelation[] rels, TableColumn[] cols)
        {
            int i = 0;
            var captionRow = cols.Length * 2 + 1;
            var relIds = rels.SelectMany(x => new[] { x.ForeignColumnId, x.MasterColumnId }).Distinct().ToArray();

            static string GetControlSelectionProperty(TableColumn tt)
            {
                if (tt.Type.ToLower().Contains("date"))
                {
                    return "SelectedDate";
                }
                else if (tt.Type.ToLower().Contains("bool"))
                {
                    return "IsChecked";
                }
                else if (tt.Type.ToLower().Contains("dates"))
                {
                    return "SelectedDate";
                }
                else
                {
                    return "Text";
                }
            }


            static string GetControlTypeProperty(TableColumn tt, string[] rels)
            {
                if (tt.IsPrimary)
                    return "TextBox";

                if (tt.Type.ToLower().Contains("date"))
                {
                    return "DatePicker";
                }
                else if (tt.Type.ToLower().Contains("bool"))
                {
                    return "CheckBox";
                }
                else if (tt.Type.ToLower().Contains("string") || rels.Any())
                {
                    return "AutoCompleteBox";
                }
                else
                {
                    return "TextBox";
                }
            }

            var captionLabelsBindings = new List<string>();
            var checkMethods = new List<string>();
            var colvs = cols.Select(col =>
            {
                var ff = ViewMaker.GetControlText(col, i += 2, rels, captionRow, out bool hasCaption);

                if (col.DataType == 6)
                {
                    checkMethods.Add($"\r\n            CheckAmount({ff[1].Titleize().MakeName()}, disposal);");
                }
                if ((col.ColumnName?.ToLower().Contains("month") ?? false) &&
               !Regex.IsMatch(col.ColumnName, "[0-9]", RegexOptions.IgnoreCase))
                {
                    checkMethods.Add($"\r\n            CheckMonth({ff[1].Titleize().MakeName()}, disposal);");
                }
                if (hasCaption)
                    if (hasCaption)
                    {
                        captionRow++;
                        if (ff.Length == 3)
                        {
                            captionLabelsBindings.Add($@"
            var {ff[2].Camelize()} = this.FindControl<TextBlock>(""{ff[2]}"");
            InitializeCaptionLabel({ff[2].Camelize()}, disposal, false);
            if ({ff[2].Camelize()} != null)
            {{
                Context.CurrentObject.WhenAnyValue(c => c.{col.ColumnName.MakeName()})
                   .Throttle(TimeSpan.FromMilliseconds(700))
                   .DistinctUntilChanged()
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(c =>
                   {{
                       var text = CurrencyToWordsConverter.ConvertToWords(c);
                       if (!string.IsNullOrWhiteSpace(text))
                          {ff[2].Camelize()}.Text = text;
                   }}).DisposeWith(disposal);
            }}
                            ");
                        }
                        else if (ff.Length == 4)
                        {
                            captionLabelsBindings.Add($@"
            var {ff[2].Camelize()} = this.FindControl<TextBlock>(""{ff[2]}"");
            InitializeCaptionLabel({ff[2].Camelize()}, disposal, true);
            if ({ff[2].Camelize()} != null)
            {{
                Context.CurrentObject.WhenAnyValue(c => c.{col.ColumnName.MakeName()})
                   .Throttle(TimeSpan.FromMilliseconds(700))
                   .DistinctUntilChanged()
                   .ObserveOn(RxApp.MainThreadScheduler)
                   .Subscribe(c =>
                   {{
                       var pairs = AutoCompletionData.{ff[3]}.Where(b => b.Key == c);
                       if (pairs.Any())
                          {ff[2].Camelize()}.Text = pairs.First().Value;
                   }}).DisposeWith(disposal);
            }}
                            ");
                        }
                    }
                var ctrName = ff[1].Titleize().MakeName();

                var foreigns = rels
                .Where(x => cols.Any(n => n.Id == x.ForeignColumnId))
                .Select(x => x.MasterColumnId).ToArray();

                var masters = rels
                              .Where(x => cols.Any(n => n.Id == x.ForeignColumnId))
                              .Select(x => x.ForeignColumnId).ToArray();

                var key = $@"        public {(ctrName.ToLower().StartsWith("txl") ? "TextBlock" : GetControlTypeProperty(col, foreigns))} {ctrName} {{get;}}
        public TextBlock {col.ColumnName.Titleize().MakeName()}ErrorBox  {{get;}}";

                var needsConverter = new[] { "AutoCompleteBox", "TextBox" }.Contains(GetControlTypeProperty(col, foreigns));
                string binding;
                if (!relIds.Contains(col.Id) && needsConverter && col.Type?.ToLower() != "string" && !col.IsPrimary)
                {
                    binding = $@"            this.Bind(ViewModel,
           viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
           view => view.{ctrName}.{GetControlSelectionProperty(col)},
           ValueConverters.From{col.Type.Titleize().MakeName()}, ValueConverters.To{col.Type.Titleize().MakeName()}).DisposeWith(disposal);";
                }
                else
                {
                    binding = $@"            this.Bind(ViewModel,
           viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
           view => view.{ctrName}.{GetControlSelectionProperty(col)}).DisposeWith(disposal);";
                }

                //     var controlAssignment = $@"            this.Bind(ViewModel,
                //viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
                //view => view.{ctrName}.{GetControlSelectionProperty(col)}).DisposeWith(disposables);";

                var ctrl = $@"            {ctrName} = this.FindControl<{(ctrName.ToLower().StartsWith("txl") ? "TextBlock" : GetControlTypeProperty(col, foreigns))}>(""{ff[1]}"");
            {col.ColumnName.Titleize().MakeName()}ErrorBox = this.FindControl<TextBlock>(""{col.ColumnName.Titleize().MakeName()}ErrorBox"");";


                string autoCompleteText = "";
                string autoCompleteAssignment = "";
                if (!col.IsPrimary && ((col.Type?.ToLower().Contains("string") ?? false) || relIds.Contains(col.Id)))
                {
                    autoCompleteText = $@"                    if (Context.AutoCompleteList.ContainsKey(nameof(k.{col.ColumnName.MakeName()})))
                        this.OneWayBind(ViewModel,
                           viewModel => viewModel.AutoCompleteList[nameof(k.{col.ColumnName.MakeName()})],
                           view => view.{ctrName}.Items).DisposeWith(disposal);";
                    var foreignTable = rels
                          .Where(x => x.ForeignColumnId == col.Id)
                          .Select(x => x.MasterColumn.Table.StrimLinedName.MakeName())
                          .FirstOrDefault();

                    if (foreignTable?.Length > 2)
                        autoCompleteAssignment = $@"                {{ nameof(k.{col.ColumnName.MakeName()}), AutoCompletionData.{foreignTable.MakeName()} }}";
                    else
                        autoCompleteAssignment = $@"                {{
                    nameof(k.{col.ColumnName.MakeName()}),
                    Context.ModelList.Select(c => c.{col.ColumnName.MakeName()}).Distinct()
                           .Select(c => new Pair(c)).ToArray()
                }}";
                }

                var errorBindings = $@"                {{
                    {ctrName}, disposable =>
                       this.BindValidation(ViewModel,
                           viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.MakeName())},
                           view => view.{col.ColumnName.Titleize().MakeName()}ErrorBox.Text)
                              .DisposeWith(disposable)
                }}";

                var contextControls = $@"                new({ctrName}, {col.ColumnName.Titleize().MakeName()}ErrorBox, ""{col.ColumnName}"", typeof({(relIds.Contains(col.Id) ? "string" : col.Type)}))";

                return new
                {
                    //TableColumn = col,
                    Key = key,
                    Binding = binding,
                    ErrorBinding = errorBindings,
                    Control = ctrl,
                    //Auto = res,
                    //ValidationLabel = validationError,
                    //Ctrl = controlAssignment,
                    AutoCompleteAssignment = autoCompleteAssignment,
                    AutoCompleteText = autoCompleteText,
                    ContextControl = contextControls
                };
            });


            if (checkMethods.Count > 4)
                checkMethods.Insert(0, "\r\n");


            var name = table.Name.MakeName();
            var singular = Functions.Singularize(name);
            var sb = new StringBuilder($@"
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pilgrims.Projects.Assistant.Contracts.Controls;
using Pilgrims.Projects.Assistant.DataEntryViewModels;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using ReactiveUI;
using Pilgrims.Projects.Assistant.Contracts;
using ReactiveUI.Validation.Extensions;
using Pilgrims.Projects.Assistant.Contracts.Messaging;
using Pilgrims.Projects.Assistant.Contracts.Classes;
using System.Linq;
using System.Reactive.Linq;
using Pilgrims.Projects.Assistant.DataEntryPages;
using Pilgrims.Projects.Assistant.Contracts.DataLayer.Models;
using Pilgrims.Projects.Assistant.DataEntry.Controls;
using Pilgrims.Projects.Assistant.Classes;

namespace Pilgrims.Projects.Assistant.DataEntry.ViewPages
{{
    public class {name}ViewPage : ViewPageBase<{name}ViewModel, I{singular}Model>
    {{
        protected override Tk GetChildControl<Tk>(string name) => this.FindControl<Tk>(name);
        private static {name}ViewPage page; 
        public static {name}ViewPage Page {{ get => page ??= new {name}ViewPage(); }}

        public {name}ViewPage()
        {{
            InitializeComponent();
            AssignViewControls();
            Buttons = this.FindControl<SmallBatchControls>(""Buttons"");
{ string.Join("\r\n", colvs.Select(c => c.Control))}
        }}

        protected override SmallBatchControls Buttons {{ get; }}
{string.Join("\r\n", colvs.Select(c => c.Key))}

        protected override List<(IControl, TextBlock, string, Type)> GetAllEntryControls()
        {{
            return new List<(IControl, TextBlock, string, Type)>
            {{
{string.Join(",\r\n", colvs.Select(c => c.ContextControl))}
            }};
        }}
 
        protected override void Loaded(CompositeDisposable disposal)
        {{
            AssignBindings(disposal);{string.Join("", checkMethods.Distinct())}
            Functions.RunOnBackground(() => AssignAutocompletions(disposal));{(captionLabelsBindings.Any() ? "\r\n            Functions.RunOnMain(() => AddCaptionLabels(disposal));" : "")}
        }}
{(captionLabelsBindings.Any() ? @"
        private void AddCaptionLabels(CompositeDisposable disposal)
        {" + string.Join("\r\n", captionLabelsBindings.Distinct()) + @"
        }
" : "")}
        private void AssignAutocompletions(CompositeDisposable disposal)
        {{  
            if (Context.AutoCompleteList == null)
                ReassignAutoCompletion();
            AutoCompletionData.HasChanged
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(x =>
                {{
                    ReassignAutoCompletion();
                }}).DisposeWith(disposal);
            Functions.RunOnMain(() =>
            {{
                try
                {{
                    var k = DataLayerFactory.CreateDataModel<I{singular}Model>();
{string.Join("\r\n", colvs.Select(c => c.AutoCompleteText).Where(b => !string.IsNullOrWhiteSpace(b)))}
                }}
                catch (Exception ex)
                {{
                    Notifier.NotifyError(""Unable to bind autocompletes"", ""Initializa Error"", ex);
                }}
            }});
        }}

        protected override void ReassignAutoCompletion()
        {{
            var k = DataLayerFactory.CreateDataModel<I{singular}Model>();
            var dic = new Dictionary<string, Pair[]>
            {{
{string.Join(",\r\n", colvs.Select(c => c.AutoCompleteAssignment).Where(b => !string.IsNullOrWhiteSpace(b)))}
            }};
            if (Context.AutoCompleteList == null)
                Context.AutoCompleteList = dic;
            else
            {{
                foreach (var item in dic)
                {{
                    if (Context.AutoCompleteList.ContainsKey(item.Key))
                        Context.AutoCompleteList[item.Key] = item.Value;
                    else Context.AutoCompleteList.Add(item.Key, item.Value);
                }}
            }}
        }}
        private void AssignBindings(CompositeDisposable disposal)
        {{
{string.Join("\r\n", colvs.Select(c => c.Binding))}
        }}

        private void InitializeComponent()
        {{
            AvaloniaXamlLoader.Load(this);
        }}
        protected override Dictionary<IControl, Action<CompositeDisposable>> AssignValidationsControls()
        {{
            return new Dictionary<IControl, Action<CompositeDisposable>>
            {{
{string.Join(",\r\n", colvs.Select(c => c.ErrorBinding))}
            }};
        }}
    }}
}}
   
            ");

            return sb.ToString();
        }

    }
}
