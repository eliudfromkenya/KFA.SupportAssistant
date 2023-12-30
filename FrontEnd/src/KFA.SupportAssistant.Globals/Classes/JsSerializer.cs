using Newtonsoft.Json;

namespace KFA.SupportAssistant.Globals.Classes;

public static class JsSerializer
{
  public static string? Serialize(object? myobj)
  {
    if (myobj == null) return null;

    var returnstring = JsonConvert.SerializeObject(myobj, new JsonSerializerSettings()
    {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    });

    return returnstring;
  }

  public static T? Deserialize<T>(string obj)
  {
    if (string.IsNullOrWhiteSpace(obj))
      return default;

    ////if (typeof(T).IsInterface)
    ////{
    ////  var type = Declarations.ServiceScope?.ServiceProvider?.GetService<T>()?.GetType();
    ////  if (type == null)
    ////    return default;

    ////  var xxx = Deserialize(obj, type);
    ////  return (T?)xxx;
    ////}

    var returnstring = JsonConvert.DeserializeObject<T>(obj);
    return returnstring;
  }

  public static object? Deserialize(string obj, Type type)
  {
    if (string.IsNullOrWhiteSpace(obj)) return null;

    var returnstring = JsonConvert.DeserializeObject(obj, type);
    return returnstring;
  }

  //public static T Deserialize<T>(byte[] value)
  //{
  //  throw new NotImplementedException();
  //}
}
