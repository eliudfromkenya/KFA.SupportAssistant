#region

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Pilgrims.ProjectManagement.Contracts;

#endregion

namespace Pilgrims.ProjectManagement.DataImport
{
    internal class Common
    {
        private static readonly Dictionary<string, string> LastNames = new Dictionary<string, string>();

        static Common()
        {
            TryToExctractWords = false;
        }

        public static bool TryToExctractWords { get; set; }

        public static string CheckNames(string name, bool nnff)
        {

            if (LastNames.ContainsKey(name))
                return LastNames[name];

            var org = name;
            if (name.ToLower() == "namespace")
                name = "App Namespace";

            name = Functions.StrimLineObjectName(name);
            var sb = new StringBuilder(name);
            foreach (Match match in Regex.Matches(name, "[a-zA-Z]{1}[0-9]{1}"))
                sb.Replace(match.Value, match.Value.Insert(1, " "));
            foreach (Match match in Regex.Matches(name, "[0-9]{1}[a-zA-Z]{1}"))
                sb.Replace(match.Value, match.Value.Insert(1, " "));
            foreach (Match match in Regex.Matches(name, "[a-z]{1}[A-Z]{1}"))
                sb.Replace(match.Value, match.Value.Insert(1, " "));

            LastNames.Add(org, name);
            return Functions.MakeAllFirstLetterCapital(name, false);
        }

        public static string CheckCaption(string name)
        {
            //name = Regex.Replace(name, "name$", "Number", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, " nos$", "Numbers", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, " no$", "Number", RegexOptions.IgnoreCase);
            return name;
            // return Functions.MakeAllFirstLetterCapital(name, false);
        }
    }
}