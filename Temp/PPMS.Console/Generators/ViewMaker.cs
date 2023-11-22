using Humanizer;
using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PPMS.Console.Generators
{

    /// <summary>
    /// Defines the <see cref="ViewMaker" />.
    /// </summary>
    internal static class ViewMaker
    {
        /// <summary>
        /// Defines the groupExempts.
        /// </summary>
        internal static string[] groupExempts = { "Cost Centres", "Ledger Accounts", "Stock Items" };

        /// <summary>
        /// Defines the selfCalculated.
        /// </summary>
        internal static string[] selfCalculated = { "computer", "calculate" };

        /// <summary>
        /// The GetEntryPageText.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="rows">The rows<see cref="string"/>.</param>
        /// <param name="controls">The controls<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetEntryPageText(string name, string rows, string controls)
        {
            return $@"<UserControl
    x:Class=""Kfa.DataEntries.DataEntry.EntryViews.{name}""
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:Custom=""http://metro.mahapps.com/winfx/xaml/controls""
    xmlns:behaviors=""clr-namespace:WPFTextBoxAutoComplete;assembly=WPFTextBoxAutoComplete""
    xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
    xmlns:enter=""clr-namespace:Kfa.DataEntries.Contracts.Convertors;assembly=Kfa.DataEntries.Contracts""
    xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
    d:DesignHeight=""450""
    d:DesignWidth=""800""
    mc:Ignorable=""d"">
 
     <Grid enter:EnterKeyTraversal.IsEnabled = ""True"" >   
           <Grid.RowDefinitions >   
               <RowDefinition Height = ""5"" />    
                <RowDefinition Height = ""Auto"" />     
                 <RowDefinition />     
                 <RowDefinition Height = ""Auto"" />      
              </Grid.RowDefinitions >
      
              <ScrollViewer Grid.Row = ""1"" >       
                   <Grid >       
                       <Grid.ColumnDefinitions >       
                           <ColumnDefinition Width = ""Auto"" />        
                            <ColumnDefinition />        
                        </Grid.ColumnDefinitions >        
                        <Grid.RowDefinitions >
                     {rows}
                </Grid.RowDefinitions >
                {controls}
            </Grid >
        </ScrollViewer >

    </Grid >
</UserControl >
";
        }

        /// <summary>
        /// The GetBehindCode.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="ctrls">The ctrls<see cref="Dictionary{TableColumn, string}"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetBehindCode(string name, Dictionary<TableColumn, string> ctrls)
        {
            var cols = string.Join(",\r\n", ctrls.Select(x => $@"                                  {{ ""{x.Key.ColumnName}"", {x.Value} }}"));
            return $@"namespace Kfa.DataEntries.DataEntry.EntryViews
{{
    using Kfa.DataEntries.Contracts;
    using Kfa.DataEntries.DataEntry.ViewModels;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for {name.MakeName()}EntryPage.xaml.
    /// </summary>
    public partial class {name.MakeName()}EntryPage : UserControl
    {{
        /// <summary>
        /// Initializes a new instance of the <see cref=""{name.MakeName()}EntryPage""/> class.
        /// </summary>
        public {name.MakeName()}EntryPage()
            {{
                InitializeComponent();
                Loaded += (xx, yy) => Functions.RunOnMain(() =>
                {{
                      if (DataContext is {name.MakeName()}ViewModel context)
                      {{
                             context.Controls = new Dictionary<string, UIElement>
                             {{
                                  {{ ""This Entry Page"", this }},   
{cols}
                             }};
                              {ctrls.FirstOrDefault(x =>
      x.Value.StartsWith("txt", StringComparison.OrdinalIgnoreCase)).Value}.Focus();
                       }}
                }}, 1000);
            }}
        }}
    }}
";
        }

        /// <summary>
        /// The GetControlCaptions.
        /// </summary>
        /// <param name="table">The table<see cref="DatabaseTable"/>.</param>
        /// <param name="rels">The rels<see cref="TableRelation[]"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetControlCaptions(DatabaseTable table, TableRelation[] rels)
        {
            var sb = new StringBuilder();
            var sbMethod = new List<string>();

            var cols = table.Columns.ToArray();
            var colIds = table.Columns.Select(x => x.Id).ToArray();
            var currencies = cols.Where(x => x.DataType == 6).ToArray();

            if (currencies.Any())
            {
                foreach (var col in currencies)
                {
                    var colName = $"{col.ColumnName} In Words".MakeName();
                    var columnName = col.ColumnName.MakeName();
                    var fieldName = Functions.MakeFirstSmallOtherLetterCapital(colName);

                    var text = $@" private string {fieldName};
        public string {colName}
        {{
            get => {fieldName}; set
            {{
                if ({fieldName} == value)
                    return;

                {fieldName} = value;
                RaisePropertyChanged(() => {colName});
            }}
        }}";
                    sb.Append(text).AppendLine().AppendLine();

                    text = $@"if (e.PropertyName == nameof(obj.{columnName}))
                {{
                    try
                    {{
                        {{
                            {colName} = """";
                            if (obj.{columnName} > 0)
                            {{
                                Functions.RunOnBackground(() =>
                                {{
                                    try
                                    {{
                                        {colName} = CurrencyToWordsConverter.ConvertToWords(obj.{columnName}.ToString());
                                    }}
                                    catch (System.Exception)
                                    {{
                                    }}
                                }});
                            }}
                        }}
                    }}
                    catch (System.Exception)
                    {{ }}
                }}";

                    sbMethod.Add(text);
                }
            }

            rels.Where(x => colIds.Contains(x.ForeignColumnId))
                .Where(x => groupExempts.Contains(x.MasterColumn.Table.Name))
            .ToList().ForEach(x =>
            {
                var master = x.MasterColumn.Table;
                var colName = x.ForeignColumn.ColumnName.MakeName();
                var propName = colName;
                var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                if (reg.Success)
                    propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                propName = propName.MakeName();
                propName = $"{propName} Name".MakeName();

                var fieldName = Functions.MakeFirstSmallOtherLetterCapital(propName);

                var text = $@" private string {fieldName};
        public string {propName}
        {{
            get => {fieldName}; set
            {{
                if ({fieldName} == value)
                    return;

                {fieldName} = value;
                RaisePropertyChanged(() => {propName});
            }}
        }}";
                sb.Append(text).AppendLine().AppendLine();



                text = $@"if (e.PropertyName == nameof(obj.{colName}))
                {{
                    try
                    {{
                        {propName} = """";
                        if (obj.{colName}?.Length >= 4)
                        {{
                            Functions.RunOnBackground(() =>
                            {{
                                try
                                {{
                                    using (var db = new DataContext())
                                        {propName} = db.{master.Name.MakeName()}
                                             .FirstOrDefault(x => x.Id == obj.{colName})?.Description?.ToUpper();
                                 
                                    if(string.IsNullOrWhiteSpace({propName}))
                                        DataEntryError = new DataEntryError
                                        {{
                                            Title = ""Missing"", Exception = new Exception(""Cannot find {master
                                            .Name.Singularize().ToLower()} with entered {x.ForeignColumn.ColumnName.ToLower()}""),
                                            Message = ""Please check if the {x.ForeignColumn.ColumnName.ToLower()} you have entered is correct""
                                        }};                                     
                                }}
                                catch (System.Exception)
                                {{
                                }}
                            }});
                        }}
                    }}
                    catch (System.Exception)
                    {{ }}
                }}";
                sbMethod.Add(text);
            });


            if (sbMethod.Any())
            {
                var body = $@"        protected override void CurrentObjectChanged(object sender, PropertyChangedEventArgs e)
        {{
            try
            {{
                if (!(CurrentObject is {table.Name.Singularize().MakeName()} obj))
                    return;

                ";

                sb.AppendLine().AppendLine().AppendLine().Append(body);
                sb.AppendLine(string.Join("\r\nelse ", sbMethod));
                sb.AppendLine(@"   }
            catch { }
        }");
            }

            return sb.ToString();
        }

        /// <summary>
        /// The GetControlText.
        /// </summary>
        /// <param name="col">The col<see cref="TableColumn"/>.</param>
        /// <param name="ordinal">The ordinal<see cref="int"/>.</param>
        /// <param name="rels">The rels<see cref="TableRelation[]"/>.</param>
        /// <returns>The <see cref="string[]"/>.</returns>
        internal static string[] GetControlText(TableColumn col, int position, TableRelation[] rels, int captionLabelRowNumber, out bool hasCaption, int controlsPerRow = 1)
        {
            var columnIndex = position % (controlsPerRow * 2);
            var ordinal = Math.Truncate((decimal)(position / (controlsPerRow * 2))) * 2;

            if (controlsPerRow > 1)
            {
                columnIndex++;
                ordinal++;
            }

            hasCaption = false;
            TableRelation master = null;
            var name = col?.ColumnName.MakeName();
            if (col.IsPrimary)
            {
                return new[] { $@"   <TextBlock
                    Grid.Row=""{ordinal}""
                    Grid.Column = ""{columnIndex}""
                    Margin = ""5""
                    Classes=""caption""
                    Text = ""{col.ColumnName}"" />
                <TextBox
                    Grid.Row = ""{ordinal}""
                    Name=""TxbId""
                    Grid.Column = ""{columnIndex + 1}""
                    Margin = ""5,5,5,5""
                    Focusable=""False""
                    VerticalAlignment = ""Top"" />
                <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal + 1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />", $"TxbId" };
            }
            else if (selfCalculated.Any(x => name.ToLower().Contains(x.ToLower())))
            {
                return new[] { $@"  
                   <WrapPanel Grid.Row=""{ordinal}"" Grid.ColumnSpan=""6"" Grid.Column = ""{columnIndex}"">
                   <TextBlock                    
                    Margin = ""5""
                    Classes=""caption""
                    Text = ""{col.ColumnName}"" />
                   <TextBlock
                    Grid.Row=""{ordinal}""
                    Name=""Txl{ name }""
                    Grid.Column=""1""
                    Margin=""5""
                    TextAlignment=""Right""
                    Classes=""SmallDescriptionscaption readonlys""
                    HorizontalAlignment=""Center""
                    Text = ""{{Binding CurrentObject.{col.ColumnName.MakeName()}}}""
                    TextWrapping=""Wrap"" >                
                  </TextBlock>
                  </WrapPanel>
                   <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal+1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />"
               , $"Txl{ name }" };
            }
            else if ((master = rels.FirstOrDefault(x => x.ForeignColumnId == col.Id)) != null)
            {
                var masterName = master.MasterColumn.Table.Name;
                string singularName = masterName.Singularize().Titleize();

                hasCaption = true;

                if (groupExempts.Contains(masterName))
                {
                    return new[] { $@"  <TextBlock
                    Grid.Row=""{ordinal}""
                    Grid.Column = ""{columnIndex}""
                    Margin = ""5""
                    Classes=""caption""
                    Text = ""{singularName}"" />
                  <Grid Grid.Row = ""{ordinal}"" Grid.Column = ""{columnIndex + 1}"" Margin=""5,5,15,5"" >   
                        <Grid.ColumnDefinitions >   
                           <ColumnDefinition />   
                           <ColumnDefinition Width = ""Auto"" />    
                           <ColumnDefinition Width = ""Auto"" />    
                        </Grid.ColumnDefinitions >    
                    <AutoCompleteBox Name=""Txt{name}""
                    Margin = ""5,5,5,5""
                    VerticalAlignment = ""Top""
                    Items = ""{{Binding AutoCompleteList.{col.ColumnName.Pluralize().MakeName()}}}""
                    Text = ""{{Binding CurrentObject.{col.ColumnName.MakeName()}}}"" />
               <Button
                Grid.Column = ""1""
                Width = ""30""
                HorizontalAlignment = ""Right""
                Command = ""{{Binding AddMasterRecordCommand}}""
                CommandParameter = ""{master.MasterColumn.Table.Name}""
                Classes=""addMaster""
                >
              <ToolTip.Tip>
                <StackPanel>
                  <StackPanel Orientation=""Horizontal"">
                    <Image Width=""24"" Height=""24"" Margin=""0,0,15,0"" Source=""avares://Pilgrims.Projects.Assistant/Assets/Icons/actions/list-add.png""/>
                    <TextBlock Classes=""h3 secondary"">Add {master.MasterColumn.Table.Name}</TextBlock>
                  </StackPanel>
                  <TextBlock Classes=""h5"">Click here to add a new {master.MasterColumn.Table.Name.ToLower()} to the database</TextBlock>
                </StackPanel>
              </ToolTip.Tip>
              </Button>

              <Button
               Grid.Column = ""2""
               Width = ""30"" Classes=""searchMaster""
               HorizontalAlignment = ""Right""
               Command = ""{{Binding SearchMasterRecordCommand}}""
               CommandParameter = ""{master.MasterColumn.Table.Name}""
              >
              <ToolTip.Tip>
                <StackPanel>
                  <StackPanel Orientation=""Horizontal"">
                    <Image Width=""24"" Height=""24"" Margin=""0,0,15,0"" Source=""avares://Pilgrims.Projects.Assistant/Assets/Icons/actions/zoom.png""/>
                    <TextBlock Classes=""h3 secondary"">Search {master.MasterColumn.Table.Name}</TextBlock>
                  </StackPanel>
                  <TextBlock Classes=""h5"">Click here to search {master.MasterColumn.Table.Name.ToLower()} from the database</TextBlock>
                </StackPanel>
              </ToolTip.Tip>
            </Button>
                </Grid >
                <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal + 1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />
                  <TextBlock
                    Grid.Row=""{captionLabelRowNumber}""
                    Grid.Column = ""0""
                    Grid.ColumnSpan=""6""
                    Classes=""caption masterView""
                    Margin=""0"" TextDecorations=""Underline""
                    Name=""Txb{singularName.MakeName()}Name""
                    HorizontalAlignment=""Center""
                    Text=""{{Binding {singularName.MakeName()}Name}}""
                    TextWrapping=""Wrap"" />", $"Txt{ name }", $"Txb{ singularName.MakeName() }Name", master.MasterColumn.Table.Name.MakeName() };
                }
                else
                {
                    return new[] { $@"  <TextBlock
                    Grid.Row=""{ordinal}""
                    Grid.Column = ""{columnIndex}""
                    Classes=""caption""
                    Margin = ""5""
                    Text = ""{singularName}"" />
                <Grid Grid.Row = ""{ordinal}"" Grid.Column = ""{columnIndex + 1}"" Margin=""5,5,15,5"" >   
                        <Grid.ColumnDefinitions >   
                           <ColumnDefinition />  
                           <ColumnDefinition Width = ""Auto"" />    
                           <ColumnDefinition Width = ""Auto"" />    
                        </Grid.ColumnDefinitions >    
                        <AutoCompleteBox
                        Margin = ""5""
                        HorizontalAlignment = ""Stretch""
                        VerticalAlignment = ""Top""
                        Name=""Cbo{name}""
                        Watermark = ""Select {singularName.ToLower()}""
                        Items = ""{{Binding AutoCompleteList.{singularName.Replace(" ", "")}Captions}}""
                        >                      
                    </AutoCompleteBox >
                    <Button
                Grid.Column = ""1""
                Width = ""30""
                HorizontalAlignment = ""Right""
                Command = ""{{Binding AddMasterRecordCommand}}""
                CommandParameter = ""{master.MasterColumn.Table.Name}""
                Classes=""addMaster""
                >
              <ToolTip.Tip>
                <StackPanel>
                  <StackPanel Orientation=""Horizontal"">
                    <Image Width=""24"" Height=""24"" Margin=""0,0,15,0"" Source=""avares://Pilgrims.Projects.Assistant/Assets/Icons/actions/list-add.png""/>
                    <TextBlock Classes=""h3 secondary"">Add {master.MasterColumn.Table.Name}</TextBlock>
                  </StackPanel>
                  <TextBlock Classes=""h5"">Click here to add a new {master.MasterColumn.Table.Name.ToLower()} to the database</TextBlock>
                </StackPanel>
              </ToolTip.Tip>
              </Button>

              <Button
               Grid.Column = ""2""
               Width = ""30"" Classes=""searchMaster""
               HorizontalAlignment = ""Right""
               Command = ""{{Binding SearchMasterRecordCommand}}""
               CommandParameter = ""{master.MasterColumn.Table.Name}""
              >
              <ToolTip.Tip>
                <StackPanel>
                  <StackPanel Orientation=""Horizontal"">
                    <Image Width=""24"" Height=""24"" Margin=""0,0,15,0"" Source=""avares://Pilgrims.Projects.Assistant/Assets/Icons/actions/zoom.png""/>
                    <TextBlock Classes=""h3 secondary"">Search {master.MasterColumn.Table.Name}</TextBlock>
                  </StackPanel>
                  <TextBlock Classes=""h5"">Click here to search {master.MasterColumn.Table.Name.ToLower()} from the database</TextBlock>
                </StackPanel>
              </ToolTip.Tip>
            </Button>
                </Grid >
            <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal + 1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />", $"Cbo{ name }" };
                }
            }
            else if (col.Type.ToLower() == "string")
            {
                string behaviors = null;
                if (col.ColumnName.MakeName().Contains("itemcode"))
                {
                    behaviors = @"
      <i:Interaction.Behaviors>
        <behaviors:AutoCompleteBoxItemCodeBehavior />
      </i:Interaction.Behaviors>";
                }
                else if (col.ColumnName.MakeName().Contains("chequen"))
                {
                    behaviors = @"
      <i:Interaction.Behaviors>
        <behaviors:AutoCompleteBoxChequeNumberBehavior />
      </i:Interaction.Behaviors>";
                }
                else if (col.ColumnName.MakeName().Contains("month") && !Regex.IsMatch(col.ColumnName, "[0-9]", RegexOptions.IgnoreCase))
                {
                    behaviors = @"  
      <i:Interaction.Behaviors>
        <behaviors:AutoCompleteBoxMonthBehavior />
      </i:Interaction.Behaviors>";
                }
                return new[] { $@"  <TextBlock
                    Grid.Row=""{ordinal}""
                    Grid.Column = ""{columnIndex}""
                    Margin = ""5""
                    Classes=""caption""
                    Text = ""{col.ColumnName}"" />
                <AutoCompleteBox
                    Grid.Row = ""{ordinal}""
                    Grid.Column = ""{columnIndex + 1}""
                    Name=""Txt{name}""
                    Margin = ""5,5,5,5""
                    VerticalAlignment = ""Top"">
                 {behaviors}</AutoCompleteBox >
                <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal + 1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />", $"Txt{ name }" };
            }
            else if (col.Type.ToLower().Contains("date"))
            {
                return new[] { $@" <TextBlock
                    Grid.Row=""{ordinal}""
                    Grid.Column = ""{columnIndex}""
                    Margin = ""5""
                    Classes=""caption""
                    Text = ""{col.ColumnName}"" />
       <AutoCompleteBox
       Grid.Row = ""{ordinal}""
        Grid.Column = ""{columnIndex + 1}""
        Name=""Dt{name}""
        Margin = ""5,5,5,5""
        VerticalAlignment = ""Top"" >
      <i:Interaction.Behaviors>
        <behaviors:AutoCompleteBoxDateBehaviour />
      </i:Interaction.Behaviors>
    </AutoCompleteBox >               
                <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal + 1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />", $"Dt{ name }" };
            }
            else if (col.Type.ToLower() == "bool")
            {
                return new[] { $@" <TextBlock
                    Grid.Row=""{ordinal}""
                    Grid.Column = ""{columnIndex}""
                    Classes=""caption""
                    Margin = ""5""
                    Text = ""{col.ColumnName}"" />
                <CheckBox
                    Grid.Row = ""{ordinal}""
                    Grid.Column = ""{columnIndex + 1}""
                    Name=""chk{name}""
                    Margin = ""5,5,5,5""
                    VerticalAlignment = ""Top""
                    IsChecked = ""{{Binding CurrentObject.{col.ColumnName.MakeName()}}}"" >         
                </CheckBox > 
                <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal + 1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />", $"chk{ name }" };
            }
            else
            {
                hasCaption = col.DataType == 6;
                var label = hasCaption ? $@"

                  <TextBlock
                    Grid.Row=""{captionLabelRowNumber}""
                    Grid.Column = ""0""
                    Grid.ColumnSpan=""6""
                    Classes=""caption masterView""
                    Margin=""0""
                    Name=""Txb{col.ColumnName.MakeName()}InWords""
                    HorizontalAlignment=""Center""
                    FontSize=""18""
                    Text=""{{Binding {col.ColumnName.MakeName()}InWords}}""
                    TextWrapping=""Wrap"" />" : "";


                return new[] { $@" <TextBlock
                    Grid.Row=""{ordinal}""
                    Grid.Column = ""{columnIndex}""
                    Classes=""caption""
                    Margin = ""5""
                    Text = ""{col.ColumnName}"" />
                <AutoCompleteBox
                    Grid.Row = ""{ordinal}""
                    Grid.Column = ""{columnIndex + 1}""
                    Name=""Txn{name}""
                    Margin = ""5,5,5,5""
                    VerticalAlignment = ""Top""
                    Items = ""{{Binding AutoCompleteList.{col.ColumnName.Pluralize().MakeName()}}}""
                    Text = ""{{Binding CurrentObject.{col.ColumnName.MakeName()}}}"" >
                    <i:Interaction.Behaviors>
                      <behaviors:{(hasCaption ? "AutoCompleteBoxCurrencyBehavior" : "AutoCompleteBoxNumberBehavior")} />
                    </i:Interaction.Behaviors>
                </AutoCompleteBox >

                 <TextBlock Name=""{name}ErrorBox"" Classes=""validationError viewPageError""
                    Grid.Row=""{ordinal + 1}""
                    Grid.Column = ""{columnIndex}""
                    Grid.ColumnSpan=""2""
                    Margin = ""0""
                    Text = """" />
                 {label}", $"Txn{name}", $"Txb{ col.ColumnName.MakeName() }InWords" };
            }
        }

        /// <summary>
        /// The GetPage.
        /// </summary>
        /// <param name="table">The table<see cref="DatabaseTable"/>.</param>
        /// <returns>The <see cref="string[]"/>.</returns>
        internal static string[] GetPage(DatabaseTable table, TableRelation[] rels)
        {

            var objs = string.Join("\r\n\r\n", table.Columns.Where(x => !x.IsPrimary)
                .Select(tt =>
                {
                    DatabaseTable master = null;

                    var propName = tt.ColumnName;
                    var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                        propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                    if ((master = rels.FirstOrDefault(x => x.ForeignColumnId == tt.Id)?.MasterColumn.Table) != null)
                        return new { Name = propName, tt };
                    return new { Name = tt.ColumnName, tt };
                }).Select(x =>
                {

                    var format = "";
                    if (x.tt.DataType == 6)
                    {
                        format = @", StringFormat='{}{0:0.00}'";
                    }
                    else if (x.tt.Type.ToLower().Contains("date"))
                    {
                        format = @", StringFormat='{}{0:MMM dd, yyyy}'";
                    }
                    //else if (x.tt.Type.ToLower().Contains("bool"))
                    //{
                    //    return
                    //  $@"            <GridViewColumn
                    //  Width=""200""
                    //  DisplayMember ""{{Binding = ""{{conv:Binding ({x.Name.MakeName()}?\'Yes\':\'No\')}}}}""
                    //  Header = ""{x.Name}"" /> ";
                    //}
                    return
                    $@"            <GridViewColumn
                    Width=""200""
                    DisplayMemberBinding = ""{{Binding Model.{x.Name.MakeName()}{format}}}""
                    Header = ""{x.Name}"" /> ";
                }));

            var name = table.Name.MakeName();
            var page = $@"<UserControl
    x:Class=""Kfa.DataEntries.DataEntry.Views.{name}Page""
    xmlns = ""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x = ""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:customecontrols = ""clr-namespace:Kfa.DataEntries.DataEntry.CustomeControls""
    xmlns:d = ""http://schemas.microsoft.com/expression/blend/2008""
    xmlns:dataEntryPages = ""clr-namespace:Kfa.DataEntries.DataEntry.EntryViews""
    xmlns:mc = ""http://schemas.openxmlformats.org/markup-compatibility/2006""
    xmlns:viewModels = ""clr-namespace:Kfa.DataEntries.DataEntry.ViewModels""
    xmlns:conv=""clr-namespace:CalcBinding;assembly=CalcBinding""
    d:DesignHeight = ""450""
    d:DesignWidth = ""800""
    Visibility = ""{{Binding PageVisibility}}""
    mc:Ignorable = ""d"" > 

     <UserControl.DataContext> 
         <viewModels:{name}ViewModel />  
     </UserControl.DataContext>  

      <customecontrols:EntriesMainPage>   
           <dataEntryPages:{name}EntryPage />   
           <customecontrols:EntriesMainPage.GridColumns>
            {objs}
        </customecontrols:EntriesMainPage.GridColumns >
 
       </customecontrols:EntriesMainPage>
     </UserControl >
     ";

            var csPage = $@"namespace Kfa.DataEntries.DataEntry.Views
{{
    /// <summary>
    /// Interaction logic for {name}Page.xaml.
    /// </summary>
    public partial class {name}Page
    {{
        /// <summary>
        /// Initializes a new instance of the <see cref=""{name}Page""/> class.
        /// </summary>
        public {name}Page()
        {{
            InitializeComponent();
        }}

        /// <summary>
        /// Defines the _page.
        /// </summary>
        private static {name}Page _page;

        /// <summary>
        /// Gets the Page.
        /// </summary>
        public static {name}Page Page
        {{
            get {{ return _page ?? (_page = new {name}Page()); }}
        }}
    }}
}}
";
            return new[] { page, csPage };
        }

        /// <summary>
        /// The GetViewModel.
        /// </summary>
        /// <param name="table">The table<see cref="DatabaseTable"/>.</param>
        /// <param name="rels">The rels<see cref="TableRelation[]"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetViewModel(DatabaseTable table, TableRelation[] rels)
        {
            try
            {
                var colIds = table.Columns.Select(x => x.Id).ToArray();
                var name = table.Name.MakeName();
                var singularName = table.Name.Singularize().MakeName();
                var singularCaption = table.Name.Singularize().ToLower();
                var caption = table.Name.ToLower();
                var primaryKey = table.Columns.FirstOrDefault(x => x.IsPrimary);
                var autoComplete = new StringBuilder();
                var populateListColNames = new StringBuilder();
                var loadComboboxItems = new StringBuilder();

                var xMethods = GetControlCaptions(table, rels);


                var objs = table.Columns.Select(tt =>
                {
                    string relName = null;
                    DatabaseTable master = null;

                    var propName = tt.ColumnName;
                    var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                    if (reg.Success)
                        propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                    propName = propName.MakeName();

                    if ((master = rels.FirstOrDefault(x => x.ForeignColumnId == tt.Id)?.MasterColumn.Table) != null)
                        relName = Functions.GetRelName(master.Columns.ToArray(), master, propName);


                    string template = null;

                    if (!string.IsNullOrWhiteSpace(relName))
                        template = $@"{propName}Captions = new SortedDictionary<string, string>(
                            db.{master?.Name.MakeName()}.AsNoTracking().Select(x =>
                        new
                        {{
                            x.Id,
                            Caption = x{relName.Substring(relName.IndexOf("."))}
                        }}).ToArray().ToDictionary(x => x.Id, y => y.Caption));";


                    var colCaption = $"                               x.{tt.ColumnName.MakeName()},";
                    if (!string.IsNullOrWhiteSpace(relName))
                    {
                        colCaption = $"                               {propName} = x.{relName},";
                    }
                    else if (tt.IsPrimary)
                        colCaption = $"                               {tt.ColumnName.MakeName()} = x.Id,";

                    return new
                    {
                        Template = string.IsNullOrWhiteSpace(relName) ? null : template,
                        ColCaption = colCaption
                    };
                }).ToList();

                loadComboboxItems.Append(string.Join("\r\n", objs.Where(x => !string.IsNullOrWhiteSpace(x.Template)).Select(x => x.Template)));
                populateListColNames.Append(string.Join("\r\n", objs.Where(x => !string.IsNullOrWhiteSpace(x.ColCaption)).Select(x => x.ColCaption)));


                var cols = table.Columns
                    .Where(col => col.Type.ToLower().Contains("string")
                    || rels.Any(x => x.ForeignColumnId == col.Id))
                    .Select(x =>
                    {
                        var xName = x.ColumnName.Pluralize().MakeName();
                        return new
                        {
                            Name = xName,
                            LName = xName.ToLower(),
                            Text = $"                        var {xName.ToLower()} = db.{name}.AsNoTracking().Select(x => x.{x.ColumnName.MakeName()}).Distinct().ToArray();"
                        };
                    }).ToList();
                autoComplete.AppendLine(string.Join("\r\n", cols.Select(x => x.Text)));
                autoComplete.AppendFormat("AutoCompleteList = new {{{0}}};", string.Join(", ", cols.Select(x => $"{x.Name} = {x.LName}")));



                var masters = string.Join("\r\n\r\n\r\n", rels.Where(x => colIds.Contains(x.ForeignColumnId))
                    .ToList().Select(x =>
                    {

                        var propName = x.ForeignColumn.ColumnName.MakeName();
                        var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                        if (reg.Success)
                            propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                        propName = propName.MakeName();


                        return $@"        /// <summary>
        /// Defines the {propName.Camelize()}Captions.
        /// </summary>
        private SortedDictionary<string, string> {propName.Camelize()}Captions;

        /// <summary>
        /// Gets or sets the {propName}Captions.
        /// </summary>
        public SortedDictionary<string, string> {propName}Captions
        {{
            get {{ return {propName.Camelize()}Captions; }}
            set
            {{
                {propName.Camelize()}Captions = value;
                RaisePropertyChanged(() => {propName}Captions);
            }}
        }}";
                    }));

                var includes = string.Join("\r\n", rels.Where(x => colIds.Contains(x.ForeignColumnId))
                   .Select(x =>
                   {
                       var propName = table.Columns.First(m => m.Id == x.ForeignColumnId).ColumnName.MakeName();
                       var reg = Regex.Match(propName, "(id|Code|Key|Number|No) *$", RegexOptions.IgnoreCase);
                       if (reg.Success)
                           propName = propName.Trim().Substring(0, propName.Length - reg.Value.Length);
                       propName = propName.MakeName();

                       return $@"                           .Include(x => x.{propName})";
                   }));




                return $@"namespace Kfa.DataEntries.DataEntry.ViewModels
{{
    using Kfa.DataEntries.Data;
    using Kfa.DataEntries.Data.Models;
    using Kfa.DataEntries.DataEntry.Classes;
    using Kfa.DataEntries.DataEntry.Classes.NumberTest;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using DataContext = Data.DataContext;

    /// <summary>
    /// Defines the <see cref=""{name}ViewModel"" />.
    /// </summary>
    public class {name}ViewModel : MainViewModel
    {{
        {masters}

        /// <summary>
        /// Gets or sets the AutoCompleteList.
        /// </summary>
        public dynamic AutoCompleteList {{ get; set; }}

{xMethods}
        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name=""parameter"">The parameter<see cref=""object""/>.</param>
        public override void Delete(object parameter)
        {{
            try
            {{
                using (var db = new DataContext())
                {{
                    var obj = db.{name}.FirstOrDefault(x => x.Id == CurrentObject.Id);
                    if (obj == null)
                        throw new KeyNotFoundException(""The {singularCaption} to delete can not be found"");

                    db.{name}.Remove(obj);
                    db.SaveChanges();

                    ShowMessage(""Deleting Successiful"", ""Successifully deleted the {singularCaption}"",
                        Contracts.Messaging.MessageTypes.Success);
                    MoveByItemIndex(CurrentItemIndex, false);
                    OverrideChanges = true;
                    Reset();
                    OverrideChanges = false;
                    RaisePropertyChanged(() => CurrentObject);
                    Functions.RunOnBackground(PopulateList);
                }}
            }}
            catch (Exception ex)
            {{
                DataEntryError = new DataEntryError
                {{
                    Title = ""Deleting Error"",
                    Exception = ex,
                    Message = ""Unable to delete {singularCaption} data from the database""
                }};
            }}
        }}

        /// <summary>
        /// The Save.
        /// </summary>
        public override void Save()
        {{
            try
            {{
                string msg;
                using (var db = new DataContext())
                {{
                    {singularName} obj = null;
                    if ((obj = db.{name}.FirstOrDefault(x => x.Id == CurrentObject.Id)) != null)
                    {{
                        MergeObject(CurrentObject, obj);
                        obj.___Tag___ = CurrentObject.___Tag___;
                        msg = ""Successifully updated the {singularCaption}"";
                    }}
                    else
                    {{
                        db.{name}.Add(({singularName})CurrentObject);
                        msg = ""Successifully added the {singularCaption}"";
                    }}

                    db.SaveChanges();
                    OverrideChanges = true;
                    Reset();
                    OverrideChanges = false;
                    ShowMessage(""Saving Successiful"", msg, Contracts.Messaging.MessageTypes.Success);
                    Functions.RunOnBackground(PopulateList);
                }}
            }}
            catch (Exception ex)
            {{
                DataEntryError = new DataEntryError
                {{
                    Title = ""Saving Data Error"",
                    Exception = ex,
                    Message = ""Unable to save {singularCaption} to the database""
                }};
            }}
        }}

        /// <summary>
        /// The CreateNewObject.
        /// </summary>
        protected override void CreateNewObject()
        {{
            CurrentObject = new {singularName}();
            ModelId = CurrentObject.GetNewId();
            RaisePropertyChanged(() => ModelId);
            IsUpdating = false;
        }}

        /// <summary>
        /// The GetModel.
        /// </summary>
        /// <param name=""id"">The id<see cref=""object""/>.</param>
        /// <returns>The <see cref=""GenBaseModel""/>.</returns>
        protected internal override GenBaseModel GetModel(object id)
        {{
            using (var db = new DataContext())
            {{
                var itm = db.{name}
                    .AsNoTracking(){includes}
                    .Include(x => x.RecordComment.DataEntryComments)
                    .Include(x => x.RecordVerification.Verifications)
                    .FirstOrDefault(x => x.Id == id.ToString());
                return itm;
            }}
        }}

        /// <summary>
        /// The PopulateList.
        /// </summary>
        protected override void PopulateList()
        {{
            Functions.RunOnBackground(() =>
            {{
                try
                {{
                    using (var db = new DataContext())
                    {{
                        var query = db.{name}
                           .AsNoTracking(){includes}
                           .Include(x => x.RecordComment.DataEntryComments)
                           .Include(x => x.RecordVerification.Verifications)
                           .Select(x => new
                           {{
{populateListColNames}
                               x.RecordComment.DataEntryComments,
                               x.RecordVerification.Verifications
                           }});

                        var models = query.ToArray();
                        LoadComboboxItems();
                        LoadAutoCompleteList();

                        CurrentObjects = new List<ModelObject>(models.Select(x => new ModelObject(x, x.{primaryKey.ColumnName.MakeName()})).OrderBy(x => x.Id.IntoSortText()));
                        base.PopulateList();
                    }}
                }}
                catch (Exception ex)
                {{
                    DataEntryError = new DataEntryError
                    {{
                        Title = ""Getting Data Error"",
                        Exception = ex,
                        Message = ""Unable to get {caption} data from the database""
                    }};
                }}
            }});
        }}

        /// <summary>
        /// The LoadComboboxItems.
        /// </summary>
        private void LoadComboboxItems()
        {{
            Functions.RunOnBackground(() =>
            {{
                try
                {{
                    using (var db = new DataContext())
                    {{
                        {loadComboboxItems}
                    }}
                }}
                catch (Exception ex)
                {{
                    DataEntryError = new DataEntryError
                    {{
                        Title = ""Getting Data Error"",
                        Exception = ex,
                        Message = ""Unable to get {caption} data from the database""
                    }};
                }}
            }});
        }}

        /// <summary>
        /// The LoadAutoCompleteList.
        /// </summary>
        private void LoadAutoCompleteList()
        {{
            Functions.RunOnBackground(() =>
            {{
                try
                {{
                    using (var db = new DataContext())
                    {{
{autoComplete}
                        RaisePropertyChanged(() => AutoCompleteList);
			        }}
                }}
                catch (Exception ex)
                {{
                    DataEntryError = new DataEntryError
                    {{
                        Title = ""Getting Data Error"",
                        Exception = ex,
                        Message = ""Unable to get {caption} data from the database""
                    }};
                }}
            }});
        }}

        /// <summary>
        /// The DeleteAllChecked.
        /// </summary>
        /// <param name=""parameter"">The parameter<see cref=""object""/>.</param>
        protected internal override void DeleteAllChecked(object parameter)
        {{
            try
            {{
                using (var db = new DataContext())
                {{
                    var ids = ModelList.Where(x => x.Checked).Select(x => x.Id).ToArray();
                    var objs = db.{name}.Where(x => ids.Contains(x.Id)).ToArray();
                    if (objs == null || !objs.Any())
                        throw new KeyNotFoundException(""The {singularCaption} to delete can not be found"");

                    db.{name}.RemoveRange(objs);
                    db.SaveChanges();

                    ShowMessage(""Deleting Successiful"", ""Successifully deleted {caption} from the database"",
                        Contracts.Messaging.MessageTypes.Success);
                    MoveByItemIndex(CurrentItemIndex, false);
                    RaisePropertyChanged(() => CurrentObject);
                    Reset();
                    Functions.RunOnBackground(PopulateList);
                }}
            }}
            catch (Exception ex)
            {{
                DataEntryError = new DataEntryError
                {{
                    Title = ""Deleting Error"",
                    Exception = ex,
                    Message = ""Unable to delete {caption} data from the database""
                }};
            }}
        }}
    }}
}}
";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
