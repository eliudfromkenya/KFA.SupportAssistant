using System.Globalization;
using KFA.SupportAssistant.Globals;

public abstract record BaseDTO<T> where T : BaseModel
{
  public string? Id { get; set; }
  public DateTime? DateUpdated___ { get; set; }
  public DateTime? DateInserted___ { get; set; }
  public object? ___Tag___ { get; set; }
  public abstract T? ToModel();

  public static decimal? ToDecimal(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;
    return decimal.Parse(value);
  }

  public static bool? ToBool(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;
    return bool.TryParse(value, out bool obj) && obj;
  }

  public static double? ToDouble(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;
    return double.TryParse(value, out double obj) ? obj : UnableToDecrypt<double>(value);
  }

  public static int? ToInt(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;
    return int.TryParse(value, out int obj) ? obj : UnableToDecrypt<int>(value);
  }

  public static short? ToShort(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;
    return short.TryParse(value, out short obj) ? obj : UnableToDecrypt<short>(value);
  }

  public static long? ToLong(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;
    return long.TryParse(value, out long obj) ? obj : UnableToDecrypt<long>(value);
  }
  public static byte? ToByte(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return null;
    return byte.TryParse(value, out byte obj) ? obj : UnableToDecrypt<byte>(value);
  }

  public static DateTime? ToDate(string? value)

  {
    if (string.IsNullOrWhiteSpace(value))
      return null;

    var provider = new CultureInfo("en-GB");

    if (DateTime.TryParse(value, provider, out DateTime vall))
      return vall;

    if (int.TryParse(value, out int val))
      return new DateTime(val);

    return DateTime.TryParse(value, out DateTime obj) ? obj : UnableToDecrypt<DateTime>(value);
  }

  private static Tx? UnableToDecrypt<Tx>(string? original)
  {
    //throw new Exception("Unable to decrypt " + original);
    return default;
  }
}
