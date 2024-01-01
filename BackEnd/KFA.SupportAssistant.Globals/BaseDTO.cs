using System.Globalization;
using KFA.SupportAssistant.Globals;

public abstract record BaseDTO<T> where T : BaseModel
{
  public string? Id { get; set; }
  public DateTime? DateUpdated___ { get; set; }
  public DateTime? DateInserted___ { get; set; }
  public object? ___Tag___ { get; set; }
  public abstract T? ToModel();

  protected static decimal? ToDecimal(string? value)
  {
    if (value == null)
      return null;
    return decimal.TryParse(value, out decimal obj) ? obj : UnableToDecrypt<decimal>(value);
  }

  protected static bool? ToBool(string? value)
  {
    if (value == null)
      return null;
    return bool.TryParse(value, out bool obj) && obj;
  }

  protected static double? ToDouble(string? value)
  {
    if (value == null)
      return null;
    return double.TryParse(value, out double obj) ? obj : UnableToDecrypt<double>(value);
  }

  protected static int? ToInt(string? value)
  {
    if (value == null)
      return null;
    return int.TryParse(value, out int obj) ? obj : UnableToDecrypt<int>(value);
  }

  protected static short? ToShort(string? value)
  {
    if (value == null)
      return null;
    return short.TryParse(value, out short obj) ? obj : UnableToDecrypt<short>(value);
  }

  protected static long? ToLong(string? value)
  {
    if (value == null)
      return null;
    return long.TryParse(value, out long obj) ? obj : UnableToDecrypt<long>(value);
  }
  protected static byte? ToByte(string? value)
  {
    if (value == null)
      return null;
    return byte.TryParse(value, out byte obj) ? obj : UnableToDecrypt<byte>(value);
  }

  protected static DateTime? ToDate(string? value)

  {
    if (value == null)
      return null;

    return null;

    //if (DateTime.TryParseExact(value, "MM/dd/yyyy HH:mm:ss tt", out DateTime valp))
    //  return valp;


    //var provider = new CultureInfo("en-GB");


    //if(DateTime.TryParse(value, provider, out DateTime vall))
    //  return vall;

    //if (int.TryParse(value, out int val))
    //  return new DateTime(val);

   // return DateTime.TryParse(value, out DateTime obj) ? obj : UnableToDecrypt<DateTime>(value);
  }

  private static Tx? UnableToDecrypt<Tx>(string? original)
  {
    //throw new Exception("Unable to decrypt " + original);
    return default;
  }
}
