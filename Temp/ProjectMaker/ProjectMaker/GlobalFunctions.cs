#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Pilgrims.ProjectManagement.Contracts.Classes;

#endregion

namespace Pilgrims.ProjectManagement.Contracts
{
    public static class Functions
    {
        public static Func<string, int> GetNumber =
            str => Convert.ToInt32(new string(str.Where(char.IsDigit).ToArray()));

        public static Func<string, string> GetLetters = str => new string(str.Where(char.IsLetter).ToArray());

        private static object cmdLock = new object();

        public static Func<string, string> GetName = str =>
        {
            var ans = new StringBuilder();
            foreach (var m in str.Where(m => !char.IsDigit(m)))
                ans.Append(char.IsLetter(m) ? m : '_');
            return ans.ToString();
        };

        //public static void RunOnBackground(Action action)
        //{
        //    BackgroundWorker worker;
        //    RunOnBackground(out worker, action);
        //}

        private static readonly object BackgroundlockObject = new object();


        private static readonly object CmdLockObject = new object();


        static readonly List<Action> Actions = new List<Action>();
        public static void RegisterAsLoginFunction(Action a)
        {
            Actions.Add(a);
            //GlobalDeclarations.EventAggregator
            //    .GetEvent<SystemMessageEvent>()
            //    .Subscribe(x =>
            //    {
            //        if (x == SystemMessage.LoggedIn)
            //            a();
            //    });
        }

        public static void RunLoginFunctions()
        {
            Actions.Reverse();
            Actions.ForEach(x => x());
        }

        internal static bool IsNumber(string item)
        {
            int i;
            return int.TryParse(item, out i);
        }

        public static string StrimLineObjectName(string name)
        {
            var _name = "";
            return name.Where(chr => char.IsLetterOrDigit(chr) || chr == ' ' || chr == '_')
                .Aggregate(_name, (current, chr) => current + chr);
        }

        public static void Message(string message)
        {
            MessageBox.Show(message);
        }

        public static void RunOnBackground(out BackgroundWorker worker, Action action)
        {
            lock (BackgroundlockObject)
            {
                try
                {
                    var action1 = action;
                    action = () =>
                    {
                        try
                        {
                            action1();
                        }
                        catch (Exception ex)
                        {
                           ShowMessage(ex, "Unhandled Background Error");
                        }
                    };

                    var helper = new BackgroundWorkHelper();
                    worker = helper.BackgroundWorker;
                    var actions = new List<Action> {action};
                    helper.SetActionsTodo(actions);
                    helper.IsParallel = true;
                    if (helper.BackgroundWorker.IsBusy)
                        helper.SetActionsTodo(actions);
                    else
                        helper.BackgroundWorker.RunWorkerAsync();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    worker = null;
                }
            }
        }


        public static string[] GetPrefixSuffix(string tableName)
        {
            try
            {
                var obj = new StringBuilder();
                foreach (var cha in tableName)
                    obj.Append(char.IsLetterOrDigit(cha) ? cha : ' ');

                var words = obj.ToString().Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Where(x => !x.Equals("tbl", StringComparison.InvariantCultureIgnoreCase)
                                || !x.Equals("sys", StringComparison.InvariantCultureIgnoreCase)).ToArray();

                return words.Length > 1
                    ? new[] {string.Format("{0}{1}", words[0].First(), words[1].First()).ToUpper(), null}
                    : new[] {words.First().Substring(0, 2).ToUpper(), null};
            }
            catch
            {
                return new[] {"Tb", ""};
            }
        }


        public static bool Ask(string question, bool defaultValue = false)
        {
            try
            {
                defaultValue = MessageBox.Show(question, Assembly.GetExecutingAssembly().FullName,
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Question) ==
                               MessageBoxResult.Yes;
            }
            catch
            {
                // ignored
            }
            return defaultValue;
        }

        public static void RunOnBackground(Action action)
        {
            BackgroundWorker worker;
            RunOnBackground(out worker, action);
        }

        //public static void EnableDatabaseLogs(string path = null)
        //{
        //    try
        //    {
        //        if (path == null)
        //            path = Path.Combine(GlobalDeclarations.WorkspaceDirectory, "Database Sql Log.sql");

        //        try
        //        {
        //            if (File.Exists(path))
        //                File.Delete(path);
        //        }
        //        catch
        //        {
        //            // ignored
        //        }
        //        //path = Path.Combine(GlobalDeclarations.WorkspaceDirectory, string.Format("Database {0:yyyy MMM dd dddd} Log.sql", DateTime.Now));

        //        //if(File.Exists(path))
        //        //    File.Delete(path);

        //        var writer = new StreamWriter(path, true);
        //        GlobalDeclarations.SystemVariables.DataContext.Log = writer;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.ToString());
        //    }
        //}

        //public static void DisableDatabaseLogs()
        //{
        //    GlobalDeclarations.SystemVariables.DataContext.Log = null;
        //}

        public static void RunOnBackground(Action action, int sleepTime)
        {
            RunOnBackground(() =>
            {
                try
                {
                    Thread.Sleep(sleepTime);
                    action();
                }
                catch (Exception ex)
                {
                    ShowMessage(ex, "Background Error");
                }
            });
        }

        public static void RunOnMain(Action action, int sleepTime = 0, Dispatcher dispatcher = null)
        {
            RunOnBackground(() =>
            {
                Action ax = () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex, "Background Task Error");
                    }
                };
                if (sleepTime > 0)
                    Thread.Sleep(sleepTime);
                (dispatcher ?? Application.Current.Dispatcher).Invoke(DispatcherPriority.Background, ax);
            });
        }

        public static void ShowMessage(Exception ex, string v)
        {
            System.Windows.MessageBox.Show(ex.ToString());
        }

        public static string MakeAllFirstLetterCapital(string myStr, bool lowerOthers)
        {
            short i;
            var makeFirstUCase = "";

            var strArr = myStr.Split(' ');
            for (i = 0; i <= strArr.Length - 1; i++)
                try
                {
                    if (strArr[i] == string.Empty) continue;
                    var others = lowerOthers ? strArr[i].Substring(1).ToLower() : strArr[i].Substring(1);
                    var ser = strArr[i].Substring(0, 1);
                    strArr[i] = ser.ToUpper() + others;
                    makeFirstUCase = makeFirstUCase + strArr[i] + " ";
                }
                catch
                {
                    // ignored
                }

            return makeFirstUCase.Trim();
        }



        public static void Notify(string p)
        {
            //throw new NotImplementedException();
        }

        public static void SetError(object sender)
        {
            //throw new NotImplementedException();
        }

        public static void ResetError(object sender)
        {
            //throw new NotImplementedException();
        }

        public static void SetIssue(TextBox ctr)
        {
            //throw new NotImplementedException();
        }

        public static long ConvertFromDate(DateTime date)
        {
            return Convert.ToInt64(date.ToString("yyyyMMddhhmmss") + date.Millisecond);
        }

        public static DataTable SortDataTable(DataTable codes, params string[] parameters)
        {
            try
            {
                if (!parameters.Any())
                    return codes;

                codes.DefaultView.Sort = string.Join(", ", parameters);
                return codes.DefaultView.ToTable();
            }
            catch (Exception)
            {
                return codes;
            }
        }

        public static string GetRandomString(int length)
        {
            var rnd = new Random();
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var nn = rnd.Next(0, 35);
                if (nn < 10)
                    sb.Append(nn);
                else
                {
                    const int con = (byte) 'A' - 10;
                    sb.Append((char) (con + nn));
                }
            }
            return sb.ToString();
        }
    }
}