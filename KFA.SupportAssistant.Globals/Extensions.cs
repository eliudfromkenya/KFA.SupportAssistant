using Humanizer;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Globals.Classes;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace KFA.SupportAssistant;

public static class Extensions
{
  public static DateTimeOffset? ToDateTimeOffset(this DateTime? date)
  {
    if (date == null)
      return null;

    var dateTime = date.Value;
    return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
            ? DateTimeOffset.MinValue
            : new DateTimeOffset(dateTime);
  }

  public static DateTime? FromDateTimeOffset(this DateTimeOffset? date)
  {
    if (date == null)
      return null;

    return ((DateTimeOffset)date).DateTime;
  }

  public static string? GetModelName(this string? name)
  {
    name = name?.Replace(" ", "");
    var xx = name?.ToLower()?.Trim();

    return xx?.EndsWith("querymodel") ?? false
        ? name?.Trim()[..(name.Trim().Length - "querymodel".Length)]
        : StrimLineObjectName(name);
  }

  private static string StrimLineObjectName(string? name)
  {
     return Functions.StrimLineObjectName(name ?? "");
  }

  public static string MakeName(this string name) => StrimLineObjectName(name).Replace(" ", "");

  public static string MakePlural(this string name)
  {
    if (string.IsNullOrWhiteSpace(name)) return name;
    return name.Pluralize();
  }

  public static string LowerFirstLeter(this string column)
  {
    if (string.IsNullOrWhiteSpace(column))
      return column;

    return column.Camelize();
  }

  public static string MakeForeignName(this string name, string singularTypeName)
  {
    if (string.IsNullOrWhiteSpace(name)) return name;
    name = name.Trim();

    if (name.ToLower().EndsWith(" id"))
      return name[..^3].Trim();

    return singularTypeName.MakeName();
  }

  public static string MakeSingular(this string name)
  {
    if (string.IsNullOrWhiteSpace(name)) return name;
    return name.Singularize().Replace("Datum", "Data");
  }

  public static decimal GetNumber(this string str)
  {
    if (str == null)
      return 0;

    if (decimal.TryParse(new string(str.Where(char.IsDigit).ToArray()), out var ans))
      return ans;
    return 0;
  }

  public static decimal[] GetNumbers(this string str)
  {
    if (str == null)
      return Array.Empty<decimal>();

    return Regex.Matches(str, @"[0-9]+(\.[0-9]+)*").OfType<Match>()
         .Select(x => decimal.TryParse(x.Value, out var obj) ? obj : 0m)
         .ToArray();
  }

  public static string CreateMd5Hash(this string password)
  {
    using var md5 = MD5.Create();
    var retVal = md5.ComputeHash(Encoding.Unicode.GetBytes(password));
    var sb = new StringBuilder();

    for (var i = 0; i < retVal.Length; i++)
      sb.Append(retVal[i].ToString("x2"));

    return sb.ToString();
  }

  public static string CreateSHA512Hash(this string password)
  {
    using var hashAlgorithm = SHA512.Create();
    var byteValue = Encoding.UTF8.GetBytes(password);
    var byteHash = hashAlgorithm.ComputeHash(byteValue);
    return Convert.ToBase64String(byteHash);
  }

  public static bool IsPrimitiveType(this Type type)
  {
    if (type == null) return false;

    return type.IsPrimitive
        || type.IsEnum
        || type == typeof(byte[])
        || type.Equals(typeof(string))
        || type.Equals(typeof(decimal));
  }

  public static bool IsPrimitiveType(this object obj)
  {
    if (obj == null) return false;
    if (obj is not Type type) type = obj.GetType();

    return type.IsPrimitive
        || type.IsEnum
        || type == typeof(byte[])
        || type.Equals(typeof(string))
        || type.Equals(typeof(decimal));
  }

  public static Uri GetUriWithparameters(this Uri uri, Dictionary<string, string>? queryParams = null, int port = -1)
  {
    var builder = new UriBuilder(uri);
    builder.Port = port;

    if (null != queryParams && 0 < queryParams.Count)
    {
      var query = HttpUtility.ParseQueryString(builder.Query);

      foreach (var item in queryParams)
        query[item.Key] = item.Value;

      builder.Query = query.ToString();
    }

    return builder.Uri;
  }

  public static string GetUriWithparameters(string uri, Dictionary<string, string>? queryParams = null, int port = -1)
  {
    var builder = new UriBuilder(uri)
    {
      Port = port
    };

    if (null != queryParams && 0 < queryParams.Count)
    {
      var query = HttpUtility.ParseQueryString(builder.Query);

      foreach (var item in queryParams)
        query[item.Key] = item.Value;

      builder.Query = query.ToString();
    }

    return builder.Uri.ToString();
  }

 

  //public static void RegisterObject<T>(this T value) where T : class
  //{
  //  Declarations.DIServices?.AddTransient<T>();
  //}

  public static void RegisterObject<T, Y>() where Y:class, T where T: class
  {
    Declarations.DIServices?.AddTransient<T, Y>();
  }

  /// <summary>
  /// The Chunk.
  /// </summary>
  /// <typeparam name="T">.</typeparam>
  /// <param name="source">The source<see cref="IEnumerable{T}"/>.</param>
  /// <param name="size">The size<see cref="int"/>.</param>
  /// <returns>The <see cref="List{List{T}}"/>.</returns>
  public static List<List<T>> Chunk<T>(this IEnumerable<T> source, int size)
  {
    var chunks = new List<List<T>>();
    var enumerable = source as T[] ?? source.ToArray();
    var xCount = enumerable.Length / size + 2;

    for (var i = 0; i < xCount; i++)
    {
      var temp = enumerable.Skip(i * size).Take(size).ToList();
      if (temp.Any()) chunks.Add(temp);
    }

    return chunks;
  }

  /// <summary>
  /// The GetAttribute.
  /// </summary>
  /// <typeparam name="T">.</typeparam>
  /// <param name="model">The model<see cref="MemberInfo"/>.</param>
  /// <param name="inherit">The inherit<see cref="bool"/>.</param>
  /// <returns>The <see cref="Type"/>.</returns>
  public static Type? GetAttribute<T>(this MemberInfo model, bool inherit = true) where T : Attribute
  {
    return (Type?)model.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
  }

  /// <summary>
  /// The GetMemberValue.
  /// </summary>
  /// <param name="model">The model<see cref="PropertyInfo"/>.</param>
  /// <param name="obj">The obj<see cref="object"/>.</param>
  /// <returns>The <see cref="object"/>.</returns>
  public static object? GetMemberValue(this PropertyInfo model, object obj)
  {
    return model?.GetValue(obj, null);
  }

  /// <summary>
  /// The SetMemberValue.
  /// </summary>
  /// <param name="model">The model<see cref="PropertyInfo"/>.</param>
  /// <param name="obj">The obj<see cref="object"/>.</param>
  /// <param name="value">The value<see cref="object"/>.</param>
  public static void SetMemberValue(this PropertyInfo model, object obj, object value)
  {
    model.SetValue(obj, value, null);
  }

  public static Exception? InnerError(this Exception? error)
  {
    return Functions.ExtractInnerException(error);
  }

  /// <summary>
  /// The Randomize.
  /// </summary>
  /// <typeparam name="T">.</typeparam>
  /// <param name="values">The values<see cref="IEnumerable{T}"/>.</param>
  /// <returns>The <see cref="List{T}"/>.</returns>
  public static List<T> Randomize<T>(this IEnumerable<T> values)
  {
    var list = values as List<T> ?? values.ToList();
    var n = list.Count;
    var rnd = new Random();

    while (n > 1)
    {
      var k = rnd.Next(0, n) % n;
      n--;
      var value = list[k];
      list[k] = list[n];
      list[n] = value;
    }

    return list;
  }

  public static string? IntoSortText(this object value)
  {
    var str = value?.ToString();

    if (str?.Contains('-') == true)
      str = Convert.ToInt32(new string(str.Substring(str.LastIndexOf('-'))
          .Where(char.IsDigit).ToArray())).ToString("00000000000000000000000000000000000000000");

    return str;
  }

  /// <summary>
  /// The ToDateTime.
  /// </summary>
  /// <param name="dateTime">The dateTime<see cref="long?"/>.</param>
  /// <returns>The <see cref="DateTime?"/>.</returns>
  public static DateTime? ToDateTime(this long? dateTime)
  {
    return dateTime == null ? null : ConvertToDateTimeString(dateTime);
  }

  /// <summary>
  /// The ToDateTime.
  /// </summary>
  /// <param name="dateTime">The dateTime<see cref="int?"/>.</param>
  /// <returns>The <see cref="DateTime?"/>.</returns>
  public static DateTime? ToDateTime(this int? dateTime)
  {
    return dateTime == null ? null : ConvertToDateTimeString(dateTime);
  }

  /// <summary>
  /// The ToDateTime.
  /// </summary>
  /// <param name="dateTime">The dateTime<see cref="long"/>.</param>
  /// <returns>The <see cref="DateTime"/>.</returns>
  public static DateTime ToDateTime(this long dateTime) => ConvertToDateTimeString(dateTime);

  /// <summary>
  /// The ToDateTime.
  /// </summary>
  /// <param name="dateTime">The dateTime<see cref="int"/>.</param>
  /// <returns>The <see cref="DateTime"/>.</returns>
  public static DateTime ToDateTime(this int dateTime) => ConvertToDateTimeString(dateTime);

  /// <summary>
  /// The FromDateTime.
  /// </summary>
  /// <param name="dateTime">The dateTime<see cref="DateTime?"/>.</param>
  /// <returns>The <see cref="long?"/>.</returns>
  public static long? FromDateTime(this DateTime? dateTime)
  {
    return dateTime == null ? null : ((DateTime)dateTime).FromDateTime();
  }

  /// <summary>
  /// Defines the strimLinedTableNames.
  /// </summary>
  internal static readonly SortedDictionary<string, string> strimLinedTableNames = new SortedDictionary<string, string>();

  /// <summary>
  /// The StrimLineTableName.
  /// </summary>
  /// <param name="tableName">The tableName<see cref="string"/>.</param>
  /// <returns>The <see cref="string"/>.</returns>
  public static string StrimLineTableName(this string tableName)
  {
    lock (strimLinedTableNames)
    {
      if (strimLinedTableNames.ContainsKey(tableName))
        return strimLinedTableNames[tableName];

      var name = tableName;

      if (tableName.StartsWith("tbl_") || tableName.StartsWith("sys_"))
        name = tableName.Substring(4);

      name = Functions.StrimLineObjectName(name);

      name = string.Join(" ", name.Replace("_", " ")
        .Split(' ').Select(x => x.Titleize()));

      strimLinedTableNames.Add(tableName, name);
      return name;
    }
  }

  // public static List<List<T>> Chunk<T>(this IEnumerable<T> source, int size)
  // {
  //    var chunks = new List<List<T>>();
  //    var enumerable = source as T[] ?? source.ToArray();
  //    var xCount = (enumerable.Length / size) + 2;

  //    for (var i = 0; i < xCount; i++)
  //    {
  //        var temp = enumerable.Skip(i * size).Take(size).ToList();
  //        if (temp.Any())
  //            chunks.Add(temp);
  //    }
  //    return chunks;
  // }

  public static long FromDateTime(this DateTime dateTime)
  {
    return
        Convert.ToInt64(
            string.Format("{0:yyyyMMddHHmmss}{1}0000000", dateTime, dateTime.Millisecond)
                .Substring(0, 17));
  }

  public static T UpdateFromDictionary<T>(this T obj, object updateData, string primaryKeyName = "Id") where T : new()
  {
    if (updateData != null && obj != null && updateData is IDictionary<string, object> updates)
    {
      var type = typeof(T).GetProperties();

      foreach (var item in updates)
      {
        var prop = type.FirstOrDefault(x => x.Name.ToUpper() == item.Key.ToUpper());

        if (prop?.CanWrite == true && item.Key.ToLower() != primaryKeyName.ToLower())
          prop?.SetValue(obj, Convert.ChangeType(item.Value, prop.PropertyType), null);
      }
    }

    return obj;
  }

  public static Dictionary<string, object?> ToUpdateDictionary<T>(this T obj) where T : new()
  {
    if (obj == null) return new();
    var data = new Dictionary<string, object>();

    return typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Where(x => x.CanWrite && x.CanRead &&
           (x?.PropertyType?.IsPrimitiveType() ?? false) && !x.Name.Contains("___tableName___"))
        .ToDictionary(x => x.Name, y => y.GetValue(obj));
  }

  public static (string?, PropertyInfo, object?)[] ToColumnValues<T>(this T obj) where T : new()
  {
    if (obj == null) return Array.Empty<(string?, PropertyInfo, object?)>();
    var data = new Dictionary<string, object>();

    return typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        .Where(x => x.CanWrite && x.CanRead &&
           (x?.PropertyType?.IsPrimitiveType() ?? false) && !x.Name.Contains("___tableName___"))
        .Select(x => (x.GetCustomAttribute<ColumnAttribute>()?.Name, x, x?.GetValue(obj)))
        .ToArray();
  }

  public static T Merge<T>(this T obj, object updateData) where T : new()
  {
    if (updateData == null) return obj;

    var props = updateData.GetType().GetProperties().Where(x => x.CanRead).ToList();
    var tProps = typeof(T).GetProperties().Where(x => x.CanWrite).ToList();

    props.ForEach(tt =>
    {
      var prop = tProps.FirstOrDefault(m => m.Name == tt.Name || m.Name == "SetId" && tt.Name == "Id");

      if (prop != null)
      {
        var value = tt.GetValue(updateData);
        prop.SetValue(obj, value);
      }
    });

    return obj;
  }

  public static T? DictionaryToObject<T>(this IDictionary<string, object> obj) where T : class, new()
  {
    if (obj != null)
    {
      var type = typeof(T);
      var ret = new T();
      var props = type.GetProperties().ToArray();

      foreach (var item in obj)
      {
        try
        {
          var val = item.Value;

          if (item.Key.Contains("_Caption"))
          {
            if (item.Value == null) continue;

            var typ = JsonConvert
                .DeserializeObject<Dictionary<string, object>>(item.Value?.ToString() ?? "");

            if (typ?.Any() ?? false) val = typ.Values.FirstOrDefault();
          }

          var prop = props.FirstOrDefault(x => x.Name == item.Key) ?? props.FirstOrDefault(x => x.Name.ToLower() == item.Key.ToLower());
          prop?.SetValue(ret, val, null);
        }
        catch (Exception ex)
        {
          var ds = item.GetType().ToString();
          Notifier.NotifyError(ex.Message, "Converting object Error", ex);
        }
      }
      return ret;
    }

    return null;
  }

  private static Type? GetUnderlyingType(Type typeToCheck)
  {
    if (typeToCheck.IsGenericType &&
        typeToCheck.GetGenericTypeDefinition() == typeof(Nullable<>))
    {
      return Nullable.GetUnderlyingType(typeToCheck);
    }
    else
    {
      return typeToCheck;
    }
  }

  private static bool IsTypeASimpleType(Type typeToCheck)
  {
    var typeCode = Type.GetTypeCode(GetUnderlyingType(typeToCheck));

    switch (typeCode)
    {
      case TypeCode.Boolean:
      case TypeCode.Byte:
      case TypeCode.Char:
      case TypeCode.DateTime:
      case TypeCode.Decimal:
      case TypeCode.Double:
      case TypeCode.Int16:
      case TypeCode.Int32:
      case TypeCode.Int64:
      case TypeCode.SByte:
      case TypeCode.Single:
      case TypeCode.String:
      case TypeCode.UInt16:
      case TypeCode.UInt32:
      case TypeCode.UInt64:
        return true;

      default:
        return false;
    }
  }

  private static readonly Dictionary<Type, PropertyInfo[]> primitiveProperties = new Dictionary<Type, PropertyInfo[]>();

  public static PropertyInfo[] GetPrimitiveProperties(this Type type)
  {
    if (primitiveProperties.ContainsKey(type))
      return primitiveProperties[type];

    var props = type.GetProperties()
      .Where(p => IsTypeASimpleType(p.PropertyType))
      .Where(pi => !(pi.Name == "___tableName___" ||
       pi.GetCustomAttributes(true)
       .Any(ca => ca.GetType() == typeof(JsonIgnoreAttribute)))).ToArray();

    primitiveProperties.Add(type, props);
    return props;

    // return type.GetProperties()
    //  .Where(p => !p.PropertyType.IsClass ||
    //   p.PropertyType == typeof(byte[]) ||
    //   p.PropertyType == typeof(string))
    //  .Where(pi => !(pi.Name == "tableName" ||
    //   pi.GetCustomAttributes(true).Any(ca => ca.GetType() == typeof(JsonIgnoreAttribute)))).ToArray();
  }

  private static readonly string[] exempts = { "OriginatorId", "DateUpdated", "DateInserted", "Id" };

  public static Dictionary<string, object?> GetPropertyDifferences<T>(this T obj)
  {
    string? json = null;
    dynamic? mm = obj;
    var ans = new Dictionary<string, object?>();

    if (string.IsNullOrWhiteSpace(json = mm?.___Tag___ as string))
      return ans;

    var old = JsonConvert.DeserializeObject<T>(json);

    typeof(T)
        .GetPrimitiveProperties()
        .Where(prop => !exempts.Contains(prop.Name))
        .ToList()
        .ForEach(prop =>
        {
          try
          {
            if (prop.CanRead)
            {
              var oldValue = prop.GetValue(old, null);

              if (oldValue != prop.GetValue(obj, null))
                ans.Add(prop.Name, oldValue);
            }
          }
          catch { }
        });

    return ans;
  }

  internal static DateTime ConvertToDateTimeString(object? obj)
  {
    try
    {
      var bb = new Queue<char>(obj?.ToString()?
          .Where(char.IsDigit)
          .Aggregate("", (current, chr) => current + chr) ?? "");

      if (bb.Count < 8)
        throw new InvalidCastException("Invalid number to be casted to date");

      for (var i = 0; i < 10; i++)
        bb.Enqueue('0');

      var sb = new StringBuilder();

      sb.Append(bb.Dequeue()).Append(bb.Dequeue()).Append(bb.Dequeue()).Append(bb.Dequeue()).Append(",");
      sb.Append(bb.Dequeue()).Append(bb.Dequeue()).Append(",");
      sb.Append(bb.Dequeue()).Append(bb.Dequeue()).Append(",");
      sb.Append(bb.Dequeue()).Append(bb.Dequeue()).Append(",");
      sb.Append(bb.Dequeue()).Append(bb.Dequeue()).Append(",");
      sb.Append(bb.Dequeue()).Append(bb.Dequeue()).Append(",");
      sb.Append(bb.Dequeue()).Append(bb.Dequeue()).Append(bb.Dequeue());

      return ConvertToDateTime(sb.ToString().Split(',').Select(x => Convert.ToInt16(x)).ToArray());
    }
    catch (Exception ex)
    {
      Notifier.NotifyError(ex, string.Format("To Date Conversion error of {0}", obj));
      return DateTime.MinValue;
    }
  }

  private static DateTime ConvertToDateTime(short[] ss)
  {
    try
    {
      return new DateTime(ss[0], ss[1], ss[2], ss[3], ss[4], ss[5]).AddMilliseconds(ss[6]);
    }
    catch (Exception)
    {
      try
      {
        if (ss[5] > 59)
        {
          ss[5] = (short)(ss[5] - 60);
          ss[4] = (short)(ss[4] + 1);
        }

        if (ss[4] > 59)
        {
          ss[4] = (short)(ss[4] - 60);
          ss[3] = (short)(ss[3] + 1);
        }

        if (ss[3] > 23)
        {
          ss[3] = (short)(ss[3] - 24);
          ss[2] = (short)(ss[2] + 1);
        }

        if (ss[2] > 31)
        {
          ss[2] = (short)(ss[2] - 31);
          ss[1] = (short)(ss[1] + 1);
        }

        if (ss[1] > 12)
        {
          ss[1] = (short)(ss[1] - 12);
          ss[0] = (short)(ss[0] + 1);
        }

        return new DateTime(ss[0], ss[1], ss[2], ss[3], ss[4], ss[5]).AddMilliseconds(ss[6]);
      }
      catch (Exception)
      {
        var days = 31;

        if (ss[1] + 1 == 4 || ss[1] + 1 == 6 || ss[1] + 1 == 9 || ss[1] + 1 == 11)
          days = 30;
        else if (ss[1] + 1 == 2)
          days = ss[0] % 4 == 0 && ss[1] < 12 ? 28 : 29;

        if (ss[2] > days)
        {
          ss[2] = (short)(ss[2] - days);
          ss[1] = (short)(ss[1] + 1);
        }

        if (ss[1] > 12)
        {
          ss[1] = (short)(ss[1] - 12);
          ss[0] = (short)(ss[0] + 1);
        }

        return new DateTime(ss[0], ss[1], ss[2], ss[3], ss[4], ss[5]).AddMilliseconds(ss[6]);
      }
    }
  }
}
