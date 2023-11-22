using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PPMS.Console.Generators
{
    static class Templates
    {
        public static string GetViewXamlPage(DatabaseTable table, TableRelation[] rels, TableColumn[] cols)
        {
            try
            {
                int i = 0;
                var captionRow = cols.Length * 2 + 1;
                var colvs = string.Join("\r\n\r\n", cols.Select(col =>
                {
                    var ff = ViewMaker.GetControlText(col, i += 2, rels, captionRow, out bool hasCaption);
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
             x:Class=""Pilgrims.Projects.Assistant.DataEntry.ViewPages.{name}ViewPage""
             xmlns:i=""clr-namespace:Avalonia.Xaml.Interactivity;assembly=Avalonia.Xaml.Interactivity""
             xmlns:behaviors=""clr-namespace:Pilgrims.Projects.Assistant.Contracts.Behaviors;assembly=Pilgrims.Projects.Assistant.Contracts""
             xmlns:buttons=""clr-namespace:Pilgrims.Projects.Assistant.Controls""
             xmlns:vm=""clr-namespace:Pilgrims.Projects.Assistant.DataEntryViewModels""
             FontSize=""{{Binding FontSize}}"" HorizontalAlignment=""Stretch""
             VerticalAlignment=""Stretch"" Width = ""{{Binding PageWidth}}"" Height = ""{{Binding PageHeight}}""
             xmlns:ext=""clr-namespace:Avalonia.ExtendedToolkit.Controls;assembly=Avalonia.ExtendedToolkit"">

  <UserControl.DataContext>
    <vm:{name}ViewModel />
  </UserControl.DataContext>

  <Grid ColumnDefinitions =""Auto,Auto,*,10"" RowDefinitions=""0,Auto,*,10"" VerticalAlignment=""Stretch"" HorizontalAlignment=""Stretch"">
    
    <Grid Grid.Row=""1""  HorizontalAlignment=""Stretch"" Grid.Column=""2"" ColumnDefinitions=""*,Auto,Auto,Auto,Auto,Auto"" >
        <Grid HorizontalAlignment=""Stretch"" ColumnDefinitions=""Auto,Auto,Auto,Auto,Auto,*,Auto"" >
            <Button
              Grid.Column=""0""
              Width=""30"" Name=""BtnFirstPage""
              HorizontalAlignment=""Right""
              Classes=""firstPage""
              Command=""{{Binding ListCommand}}""
              CommandParameter=""FirstPage"" />
            
            <Button  Name=""BtnPreviousPage""
              Grid.Column=""1""
              Width=""30""
              HorizontalAlignment=""Right""
              Classes=""previousPage""
              Command=""{{Binding ListCommand}}""
              CommandParameter=""PreviousPage"" />
            
            <TextBlock  Name=""TxbPageNavigation""
                         Grid.Column=""2""
                         Margin=""5""
                         Classes=""caption""
                         Text=""{{Binding PageCaption}}"" />
            <Button  Name=""BtnNextPage""
              Grid.Column=""3""
              Width=""30""
              HorizontalAlignment=""Right""
              Classes=""nextPage""
              Command=""{{Binding ListCommand}}""
              CommandParameter=""NextPage"" />
            <Button  Name=""BtnLastPage""
              Grid.Column=""4""
              Width=""30""
              HorizontalAlignment=""Right""
              Classes=""lastPage""
              Command=""{{Binding ListCommand}}""
              CommandParameter=""LastPage"" />
         
            <TextBox  Name=""TxtSearchTerm""
               MinWidth=""150"" MinHeight=""30""
               MaxHeight=""{{Binding SearchBarHeight}}""
               HorizontalAlignment=""Stretch""
               VerticalAlignment=""Stretch"" 
               BorderThickness=""0"" Grid.Column=""5""
               BorderBrush=""Transparent""
               Text=""{{Binding SearchTerm}}"" />
             <Button  Name=""BtnSearch""
                Grid.Column=""6""
                Width=""30""
                HorizontalAlignment=""Right""
                Classes=""search""
                Command=""{{Binding ListCommand}}""
                CommandParameter=""Search"" />
          </Grid>
           
            <CheckBox
                Grid.Column=""1""  Name=""ChkCheckAll""
                Command=""{{Binding ListCommand}}""
                CommandParameter=""CheckAll""
                Content=""Check All"" />
            <Button  Name=""BtnDeleteChecked""
                Grid.Column=""2""
                Width=""30""
                HorizontalAlignment=""Right""
                Classes=""deleteChecked""
                Command=""{{Binding ListCommand}}""
                CommandParameter=""DeleteChecked"" />
            <Button  Name=""BtnRefresh""
                Grid.Column=""3""
                Width=""30""
                HorizontalAlignment=""Right""
                Classes=""refresh""
                Command=""{{Binding ListCommand}}""
                CommandParameter=""Refresh"" />
            <Button Name=""BtnSelectColumns""
                Grid.Column=""4""
                Width=""30""
                HorizontalAlignment=""Right""
                Classes=""selectColumns""
                Command=""{{Binding ListCommand}}""
                CommandParameter=""Select Columns"" />
            <Button  Name=""BtnExport""
                Grid.Column=""5""
                Width=""30""
                HorizontalAlignment=""Right""
                Classes=""export""
                Command=""{{Binding ListCommand}}""
                CommandParameter=""Export"" />
    </Grid >

    <Grid RowDefinitions="" *, Auto"" Grid.Row=""2"" Grid.RowSpan=""2"" Grid.Column=""0""  Name=""GdControls""
      VerticalAlignment=""Stretch"" HorizontalAlignment=""Stretch"" >
      <ScrollViewer Grid.Row=""0"" MaxHeight=""{{Binding ListHeight}}"" 
                  HorizontalAlignment =""Stretch"" >
        <Grid ColumnDefinitions=""Auto,*"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Top""
          RowDefinitions =""{gridRows}"" >
         {colvs}

          <TextBlock Classes=""caption error""
                Grid.Row=""{captionRow + 2}""
                Grid.Column=""0""
                Grid.ColumnSpan=""4""
                Margin=""0""  Name=""MessageBlock""
                HorizontalAlignment=""Center""
                FontSize=""18""
                Foreground=""{{DynamicResource ErrorBrush}}""
                Text=""{{Binding DataEntryError.Message}}""
                TextWrapping=""Wrap"" >
               <i:Interaction.Behaviors>
                  <behaviors:TextBlockMessageBehavior />
               </i:Interaction.Behaviors>
         </TextBlock>
        </Grid >
      </ScrollViewer >
  <buttons:SmallBatchControls x:Name=""Buttons""
                           Grid.Row=""1"" 
                           MaxHeight=""120""
                           Margin=""0""
                           HorizontalAlignment=""Center""
                           FontSize=""18""
                           Foreground=""{{DynamicResource ErrorBrush}}"" />
    </Grid >


    <StackPanel Grid.Column=""2"" Grid.Row=""2"" VerticalAlignment=""Stretch"" HorizontalAlignment=""Stretch"" >
      <DataGrid Name=""DgItems"" Items=""{{Binding ModelList}}"" RowHeight=""{{Binding ListRowHeight}}""
               SelectedItem =""{{Binding CurrentObject }}"" SelectionMode=""Extended"" HorizontalScrollBarVisibility=""Auto""
                MaxHeight =""{{Binding ListHeight}}"" >
        <DataGrid.Columns >
          <DataGridCheckBoxColumn
            Header="""" Binding=""{{Binding RecordIsSelected}}"" />
          <DataGridTextColumn Binding=""{{Binding Id}}"" Header=""{table.PrimaryKey.ColumnName}"" />
{string.Join("\r\n", cols.Where(x => !x.IsPrimary)
     .Select(x => x.ColumnName)
     .Select(x => $@"          <DataGridTextColumn Binding=""{{Binding {x.MakeName()}}}"" Header=""{x}"" />"))
     }
        </DataGrid.Columns >
      </DataGrid >
    </StackPanel >

    <GridSplitter Name=""GridSplitter"" Grid.Column=""1"" Grid.RowSpan=""3"" Background=""Black"" ResizeBehavior=""BasedOnAlignment"" ResizeDirection=""Auto"" VerticalAlignment=""Stretch"" />
  </Grid >
</UserControl >
";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // static List<TableColumn> tableColumns = new List<TableColumn>();
        public static string GetPageViewModel(DatabaseTable table, TableRelation[] rels, TableColumn[] cols)
        {

            var relIds = rels.Select(x => x.ForeignColumnId).ToList();
            relIds.AddRange(rels.Select(x => x.MasterColumnId));
            relIds = relIds.Distinct().ToList();

            var colFields = cols.Select(x =>
            {
                var field = $@"                            .Field(""{x.ColumnName.Camelize().MakeName()}"")";
                if (x.IsPrimary)
                    field = $@"                            .Field(""id"")";

                var propName = x.ColumnName;
                var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                if (reg.Success)
                    propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                propName = propName.MakeName().Camelize();

                var masters = $@"
                            .Field(""{ propName}"",                           
                                { propName} =>
";
                var relMasters = rels
                .Where(y => y.ForeignColumn.ColumnName == x.ColumnName)
                .Select(x => x.MasterColumn.Table)
                .Select(y =>
                 {
                     var rem = string.Join("\r\n", GetTableMasters(y).Select(m =>
                        {
                            return $@"                                    {propName}.Alias(""{propName}_Caption"").Field(""{m.ColumnName.MakeName().Camelize()}"")";
                        }));
                     return $"{masters}{rem})";
                 });
                return field + (x.IsPrimary ? "" : string.Join("\r\n", relMasters.Distinct()));
            });

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
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;

namespace Pilgrims.Projects.Assistant.ViewModels.DataEntry
{{
    public class {name}ViewModel : DataEntryViewModel<{singular}QueryModel>
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

        internal static TableColumn[] GetTableMasters(DatabaseTable table)
        {
            //if (tableColumns.Count <1)
            //{
            //    using var db = new Data.Context();
            //    tableColumns = db.Columns.Include(x => x.Table).ToList();
            //}
            if (table.Name == "Cost Centres")
            {
                return table.Columns.Where(x =>
                x.ColumnName == "Cost Centre Code" ||
                x.ColumnName == "Description").ToArray();
            }
            string[] names = { "Description", "Name", "Title", table.Name.Singularize() };
            var cols = table.Columns.Where(x => names.Contains(x.ColumnName)).ToArray();
            if (cols.Any())
                return cols;

            cols = table.Columns
                .Where(x => x.Type.ToLower().Contains("string")
                     && x.ColumnName != "Narration")
                .Take(1).ToArray();
            if (cols.Any())
                return cols;

            return new[] { table.Columns.First() };
        }


        public static string GetViewPageCodeBehind(DatabaseTable table, TableRelation[] rels, TableColumn[] cols)
        {
            int i = 0;
            var captionRow = cols.Length * 2 + 1;

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

            var checkMethods = new StringBuilder();
            var colvs = cols.Select(col =>
            {
                var ff = ViewMaker.GetControlText(col, i += 2, rels, captionRow, out bool hasCaption);
                if (hasCaption)
                    captionRow++;
                var ctrName = ff[1].Titleize().MakeName();

                if (col.DataType == 6)
                {
                    checkMethods.AppendLine($"CheckAmount({ctrName});");
                }
                if ((col.ColumnName?.ToLower().Contains("month") ?? false) &&
               !Regex.IsMatch(col.ColumnName, "[0-9]", RegexOptions.IgnoreCase))
                {
                    checkMethods.AppendLine($"CheckMonth({ctrName});");
                }

                var foreigns = rels
                .Where(x => cols.Any(n => n.Id == x.ForeignColumnId))
                .Select(x => x.MasterColumnId).ToArray();

                var masters = rels
                              .Where(x => cols.Any(n => n.Id == x.ForeignColumnId))
                              .Select(x => x.ForeignColumnId).ToArray();

                var key = $@"        public {GetControlTypeProperty(col, foreigns)} {ctrName};
        public TextBlock {col.ColumnName.Titleize().MakeName()}ErrorBox;";

                var needsConverter = new[] { "AutoCompleteBox", "TextBox" }.Contains(GetControlTypeProperty(col, foreigns));
                string binding;
                var relIds = rels.SelectMany(x => new[] { x.ForeignColumnId, x.MasterColumnId }).Distinct().ToArray();
                if (!relIds.Contains(col.Id) && needsConverter && col.Type?.ToLower() != "string" && col.IsPrimary)
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


                var controlAssignment = $@"            this.Bind(ViewModel,
           viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
           view => view.{ctrName}.{GetControlSelectionProperty(col)}).DisposeWith(disposables);";

                var ctrl = $@"            {ctrName} = this.FindControl<{(ctrName.ToLower().StartsWith("txl") ? "TextBlock" : GetControlTypeProperty(col, foreigns))}>(""{ff[1]}"");
            {col.ColumnName.Titleize().MakeName()}ErrorBox = this.FindControl<TextBlock>(""{col.ColumnName.Titleize().MakeName()}ErrorBox"");";

                string res = "";
                if (GetControlTypeProperty(col, foreigns).Contains("Auto"))
                {
                    if (masters.Contains(col.Id))
                    {
                        var propName = col.ColumnName;
                        var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                        if (reg.Success)
                            propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                        res = $@"                if(dataContext.AutoCompleteList.{propName.Titleize().MakeName()} != null)
                {ctrName}.Bind(AutoCompleteBox.ItemsProperty,
                  autoCompleteObservable.Select(x => dataContext.AutoCompleteList.{propName.Titleize().MakeName()}_Caption))
                  .DisposeWith(disposables);";
                    }
                    else
                    {
                        res = $@"                if(dataContext.AutoCompleteList.{col.ColumnName.Titleize().MakeName()} != null)
                {ctrName}.Bind(AutoCompleteBox.ItemsProperty,
                  autoCompleteObservable.Select(x => dataContext.AutoCompleteList.{col.ColumnName.Titleize().MakeName()}))
                  .DisposeWith(disposables);";
                    }
                }

                var errorBindings = $@"
            #region {col.ColumnName.Titleize().MakeName()} Validation Binding
            Observable
              .FromEventPattern<GotFocusEventArgs>({ctrName}, ""GotFocus"")
              .Subscribe(tt =>
              {{
                  if (!ctrls.Contains({ctrName}))
                  {{                        
                      Functions.RunOnMain(()=>
                      {{
                      if(!{ctrName}.IsFocused){{
                      try{{
                        this.BindValidation(ViewModel,
                          viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
                          view => view.{col.ColumnName.Titleize().MakeName()}ErrorBox.Text)
                             .DisposeWith(disposables);
                        ctrls.Add({ctrName});
                       }}catch{{}}
                       }}
                     }},1000);
                  }}
              }}).DisposeWith(disposables);
            #endregion
                ";

                var validationError = $@"            dataContext.ValidationLabels.Add({ctrName}, {col.ColumnName.Titleize().MakeName()}ErrorBox);";

                var errorBinding = $@"
            #region {col.ColumnName.Titleize().MakeName()} Validation Handler          
                        this.BindValidation(ViewModel,
                          viewModel => viewModel.CurrentObject.{(col.IsPrimary ? "Id" : col.ColumnName.Titleize().MakeName())},
                          view => view.{col.ColumnName.Titleize().MakeName()}ErrorBox.Text)
                             .DisposeWith(disposables);                       
            #endregion
                ";
                var contextControls = $@"                                  {{ ""{col.ColumnName}"", {ctrName} }}";

                return new
                {
                    TableColumn = col,
                    Key = key,
                    Binding = binding,
                    ErrorBinding = errorBindings,
                    Control = ctrl,
                    Auto = res,
                    ValidationLabel = validationError,
                    //Ctrl = controlAssignment,
                    ContextControl = contextControls
                };
            });


            var gridRows = string.Join(",", Enumerable.Repeat("Auto", captionRow + 3));
            var name = table.Name.MakeName();
            var sb = new StringBuilder($@"using Avalonia;
using Avalonia.Controls;
using Avalonia.ExtendedToolkit.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DynamicData;
using Pilgrims.Projects.Assistant.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Commands;
using Pilgrims.Projects.Assistant.Controls;
using Pilgrims.Projects.Assistant.ViewModels.DataEntry;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Pilgrims.Projects.Assistant.ViewPages
{{
    public class {name}ViewPage : ReactiveUserControl<{name}ViewModel>
    {{
        private static {name}ViewPage page;
        public BusyIndicator BusyIndicator;
        public SmallBatchControls BatchControls;
        public DataGrid DgItems;
        Exception LoadingError;
        {name}ViewModel dataContext;
        public TextBlock ErrorControl;
        IObservable<EventPattern<PropertyChangedEventArgs>> viewModelPropertyObservable;
{string.Join("\r\n", colvs.Select(c => c.Key))}

public {name}ViewPage()
        {{
            this.InitializeComponent();
            Init();
        }}

        public static {name}ViewPage Page
        {{
            get
            {{
                return page ??= new {name}ViewPage();
            }}
        }}

        private void Init()
        {{
            AssignControls();

            DataContext = dataContext = new {name}ViewModel();
            dataContext.DataGrid = DgItems;
            viewModelPropertyObservable = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>
              (handler => handler.Invoke, h => ViewModel.PropertyChanged += h, h => ViewModel.PropertyChanged -= h);
            {checkMethods}

            AssignValidationLabels();
            InitPage(dataContext);

            DataEntryExtensions.InitPageControls(this, dataContext.Controls, dataContext);
            Functions.RunOnMain(() => dataContext.Reset(), 500);
       
            WhenActivatedChecks();
        }}

        private void AssignValidationLabels()
        {{
{string.Join("\r\n", colvs.Select(c => c.ValidationLabel))}
        }}

        private void WhenActivatedChecks()
        {{
            this.WhenActivated(disposables =>
            {{
                Functions.RunOnMain(() =>
                {{
                    if (ViewModel.CurrentObject == null)
                    {{
                       ViewModel.Reset();
                    }}
                }});
                if (BatchControls?.Delete != null)
                    BatchControls.Delete.IsEnabled = false;
                if (BatchControls?.Save != null)
                    BatchControls.Save.IsEnabled = false;

                 viewModelPropertyObservable
                 .Throttle(TimeSpan.FromSeconds(0.8))
                 .DistinctUntilChanged().Subscribe(x =>
                {{
                    Functions.RunOnMain(() =>
                    {{
                        if (x.EventArgs.PropertyName == nameof(ViewModel.IsUpdating))
                        {{
                            if (BatchControls?.Save != null)
                                BatchControls.Save.Content = ViewModel.IsUpdating ? ""Update"" : ""Save"";
                            if (BatchControls?.Delete != null)
                                BatchControls.Delete.IsEnabled = ViewModel.IsUpdating;
                        }}
                        else if (x.EventArgs.PropertyName == nameof(ViewModel.IsChanged))
                        {{
                            if (BatchControls?.Save != null)
                                BatchControls.Save.IsEnabled = ViewModel.IsChanged;
                        }}
                        else if (x.EventArgs.PropertyName == nameof(ViewModel.ModelList))
                        {{
                            //dataContext.Refresh(DgItems, disposables);
                        }}
                        else if (x.EventArgs.PropertyName == nameof(ViewModel.CurrentObject))
                        {{
                            try
                            {{
                                DgItems.ScrollIntoView(ViewModel.CurrentObject, null);
                                try
                                {{
                                    foreach (var row in DgItems.SelectedItems)
                                    {{
                                    }}
                                }}
                                catch {{ }}
                            }}
                            catch {{ }}
                        }}
                    }});
                }}).DisposeWith(disposables);

                AddValidationControlsEvents(disposables);

                AddEntryControlsBinding(disposables);

                AddAutoCompletes(disposables, viewModelPropertyObservable);
                if (BusyIndicator != null)
                    this.WhenAnyValue(v => v.dataContext.IsBusy)
                         .BindTo(this, v => v.BusyIndicator.IsBusy)
                         .DisposeWith(disposables);
                //this.WhenAnyValue(v => v.dataContext)
                //         .Subscribe(tt => tt.LoadingPage(this, getModel, ""Add Page""))
                //         .DisposeWith(disposables);
            }});

        }}

        private void AddEntryControlsBinding(CompositeDisposable disposables)
        {{
            if (DgItems != null)
                this.Bind(ViewModel,
                viewModel => viewModel.CurrentObject,
                view => view.DgItems.SelectedItem).DisposeWith(disposables);
{string.Join("\r\n", colvs.Select(c => c.Binding))}
        }}

        private void AddValidationControlsEvents(CompositeDisposable disposables)
        {{
            var ctrls = new List<Control>();
{string.Join("\r\n", colvs.Select(c => c.ErrorBinding))}
        }}

        private void AssignControls()
        {{
            DgItems = this.FindControl<DataGrid>(""DgItems"");
            BusyIndicator = this.FindControl<BusyIndicator>(""BusyIndicator"");
            BatchControls = this.FindControl<SmallBatchControls>(""lblControls"");
{string.Join("\r\n", colvs.Select(c => c.Control))}
        }}

        private void AddAutoCompletes(CompositeDisposable disposables, IObservable<EventPattern<PropertyChangedEventArgs>> propertyObservable)
        {{
          propertyObservable
          .Throttle(TimeSpan.FromSeconds(7))
          .DistinctUntilChanged()
          .SubscribeOn(RxApp.MainThreadScheduler)
          .Subscribe(tt =>
          {{
            try
            {{
                var autoCompleteObservable = propertyObservable
                    .Where(x => x.EventArgs.PropertyName == nameof(dataContext.AutoCompleteList));

{string.Join("\r\n", colvs.Select(c => c.Auto))}
            }}
            catch (Exception ex)
            {{
                LoadingError = ex;
            }}
           }}).DisposeWith(disposables);
        }}

        private void InitializeComponent()
        {{
            AvaloniaXamlLoader.Load(this);
        }}

        private void InitPage({name}ViewModel context)
        {{
            if (context == null)
                return;

            var mx = new Dictionary<string, Control>
                         {{
                            {{""This Entry Page"", this }},
{string.Join(",\r\n", colvs.Select(c => c.ContextControl))}
                         }};
            context.Controls = mx.Where(x => x.Value != null).ToDictionary(x => x.Key, y => y.Value);
        }}
    }}
}}

            ");

            return sb.ToString();
        }

    }
}
