using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PPMS.Console.Models {
    internal static class Functions {
        public static string StrimLineObjectName (this string name) {
            return name.Where (chr => char.IsLetterOrDigit (chr) || chr == ' ' || chr == '_')
                .Aggregate ("", (current, chr) => current + chr);
        }

        public static string MakeName (this string name) {
            return StrimLineObjectName (name).Replace (" ", "");
        }

        static Dictionary<string, string> singulars = new Dictionary<string, string> ();
        internal static string Singularize (string name) {
            lock (singulars) {
                if (!singulars.ContainsKey (name))
                    singulars.Add (name, singularize (name));
                return singulars[name];
            }
        }

        static string singularize (string name) {
            if (name.Contains (" "))
                return string.Join (" ", name.Split (' ').Select (x => Singularize (x)));

            if (name.EndsWith ("ies"))
                return name.Substring (0, name.Length - 3) + "y";
            if (name.EndsWith ("xes"))
                return name.Substring (0, name.Length - 3);
            if (name.EndsWith ("s"))
                return name.Substring (0, name.Length - 1);
            return name;
        }

        public static string Pluralize (string name) {
            if (name.ToLower ().EndsWith ("s"))
                return name;
            else return $"{name}s";
        }
        internal static string GetRelName (TableColumn[] tableColumns, DatabaseTable master, string colName) {
            var singular = Functions.Singularize (master.Name).MakeName ();
            var singularLower = singular.ToLower ();

            Func<string, bool> CheckIfValid = name => {
                if (name.EndsWith ("id"))
                    return false;

                if (new [] { "narration", "descri" }.Any (c => name.Contains (c)))
                    return false;
                return true;
            };

            try {
                var colNames = tableColumns.Where (x => !x.IsPrimary).Select (x =>
                    new {
                        Name = x.ColumnName.MakeName ().ToLower (),
                            Caption = x.ColumnName.MakeName (),
                            Names = x.ColumnName.ToLower ().Contains (" id") ? new [] { "<L*&*&*&*>" } :
                            x.ColumnName.Trim ().ToLower ().Split (' '),
                            x.Type
                    }).ToArray ();
                var propName = colName;
                if (Regex.IsMatch (propName, "id *$", RegexOptions.IgnoreCase))
                    propName = propName.Trim ().Substring (0, propName.Length - 2);
                propName = propName.MakeName ();
                var propNameLower = propName.ToLower ();

                var cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.Contains ("name") &&
                    (x.Name.Contains (singularLower) || singularLower.Contains (x.Name)));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.Contains ("name") && x.Names.Any (y =>
                    (y.Contains (singularLower) || singularLower.Contains (y))));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.Contains ("name") &&
                    (x.Name.Contains (propNameLower) || propNameLower.Contains (x.Name)));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.Contains ("name") && x.Names.Any (y =>
                    (y.Contains (propNameLower) || propNameLower.Contains (y))));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.Equals ("name"));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && (x.Name.Contains (singularLower) || singularLower.Contains (x.Name)));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && (x.Name.Contains (propNameLower) || propNameLower.Contains (x.Name)));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.Contains ("name"));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Names.Any (y =>
                    (y.Contains (singularLower) || singularLower.Contains (y))));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Names.Any (y =>
                    (y.Contains (propNameLower) || propNameLower.Contains (y))));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.MakeName ().Contains (singularLower) ||
                    singularLower.Contains (x.Name.MakeName ()));
                if (cols.Any ())
                    cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.MakeName ().Contains (propNameLower) ||
                        propNameLower.Contains (x.Name.MakeName ()));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && x.Name.Contains ("title"));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && (x.Type == "string" &&
                    !new [] { "narration", "descri", "id" }.Any (m => x.Name.Contains (m))));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;
                cols = colNames.Where (x => CheckIfValid (x.Name) && !(
                    new [] { "narration", "descri", "id" }.Any (m => x.Name.Contains (m))));
                if (cols.Any ())
                    return propName + "." + cols.First ().Caption;

                return propName + "." + colNames.First ().Caption;
            } catch (System.Exception ex) {
                System.Console.WriteLine (string.Format ("Writting Error \r\n{0}", ex));
                return singular;
            }
        }
        public static string MakeFirstSmallOtherLetterCapital (string myStr, bool lowerAllFirstLetters = false) {
            if (!lowerAllFirstLetters)
                return string.Format ("{0}{1}", myStr[0].ToString ().ToLower (), myStr.Substring (1, myStr.Length - 1));

            short i;
            var makeFirstUCase = "";
            var strArr = myStr.Trim ().Split (' ');

            for (i = 0; i < strArr.Length; i++)
                try {
                    if (!string.IsNullOrWhiteSpace (strArr[i]))
                        makeFirstUCase += string.Format ("{0}{1} ", strArr[i][0].ToString ().ToLower (),
                            strArr[i].Substring (1, strArr[i].Length - 1));
                }
            catch {
                // ignored
            }
            return makeFirstUCase.Trim ();
        }

        public static string MakeAllFirstLetterCapital (string myStr, bool lowerOthers = true) {
            short i;
            var makeFirstUCase = "";

            var strArr = myStr.Split (' ');
            for (i = 0; i <= strArr.Length - 1; i++)
                try {
                    if (strArr[i] != string.Empty) {
                        var others = lowerOthers ? strArr[i].Substring (1).ToLower () : strArr[i].Substring (1);
                        var ser = strArr[i].Substring (0, 1);
                        strArr[i] = ser.ToUpper () + others;
                        makeFirstUCase = makeFirstUCase + strArr[i] + " ";
                    }
                }
            catch {
                // ignored
            }

            return makeFirstUCase.Trim ();
        }

        public static string GetTypes (int type) {
            string mm;
            switch (type) {
                case 2:
                    mm = "short";
                    break;
                case 3:
                    mm = "long";
                    break;
                case 4:
                    mm = "single";
                    break;
                case 5:
                    mm = "double";
                    break;
                case 7:
                    mm = "global::System.DateTime";
                    break;
                case 11:
                    mm = "bool";
                    break;
                case 17:
                    mm = "byte";
                    break;
                case 72:
                    mm = "guid";
                    break;
                case 129:
                case 133:
                    mm = "byte[]";
                    break;
                case 130:
                case 134:
                    mm = "string";
                    break;
                case 8:
                    mm = "int";
                    break;
                case 128:
                    mm = "byte[]";
                    break;
                case 6:
                case 131:
                    mm = "decimal";
                    break;
                case 15:
                    mm = "char";
                    break;

                default:
                    return "string";
            }
            return mm;
        }

        public static int ReverseTypes (string type) {
            if (type.ToLower ().Contains ("date"))
                return 7;

            int mm;
            switch (type) {
                case "short":
                    mm = 2;
                    break;
                case "long":
                    mm = 3;
                    break;
                case "single":
                    mm = 4;
                    break;
                case "double":
                    mm = 5;
                    break;
                case "bool":
                    mm = 11;
                    break;
                case "byte":
                    mm = 17;
                    break;
                case "guid":
                    mm = 72;
                    break;
                case "byte[]":
                    mm = 129;
                    break;
                case "string":
                    mm = 130;
                    break;
                case "int":
                    mm = 8;
                    break;
                case "decimal":
                    mm = 131;
                    break;
                case "char":
                    mm = 15;
                    break;

                default:
                    return 130;
            }
            return mm;
        }

        internal static string CheckCSharpFileContent (string text) {
            var importError = @"using .*[^;] *\n";
            foreach (Match obj in Regex.Matches (text, importError, RegexOptions.IgnoreCase)) {
                if (!string.IsNullOrEmpty (obj.Value) && obj.Value.Length > 6)
                    text = text.Replace (obj.Value, obj.Value.Substring (0, obj.Value.Length - 2) + ";\r\n");
            }
            importError = @"using .*\.;";
            foreach (Match obj in Regex.Matches (text, importError, RegexOptions.IgnoreCase)) {
                if (!string.IsNullOrEmpty (obj.Value) && obj.Value.Length > 6) {
                    var value = obj.Value.Trim ();
                    text = text.Replace (value, value.Substring (0, value.Length - 2) + ";\r\n");
                }
            }
            return text.Replace (";;", ";");
        }
    }
}