using KFA.SupportAssistant.Globals;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KFA.SupportAssistant.Web.Services;

public static class PatchUpdater
{
  internal static T Patch<T, X>(Func<JsonPatchDocument<T>>  getPatchDocument, string body, T tt) where T : BaseDTO<X>, new() where X : BaseModel, new()
  {
    JsonPatchDocument<T>? patchDocument = null;
    try
    {
      patchDocument = getPatchDocument();
    }
    catch { }
    if (patchDocument == null)
    {
      JObject jsonObj = JObject.Parse(body);
      var values = jsonObj?.ToObject<Dictionary<string, object?>>();
      if(values?.Count > 0)
      {
        string vals = $"[{(string.Join(",", values.Select(n => $@"{{ """"op"""": """"replace"""", """"path"""": """"/{n.Key}"""", """"value"""": """"{n.Value}"""" }}")))}]";
        patchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<T>>(vals);
      }
      //patchDocument?.ApplyTo(tt);
    }

    //if()



    patchDocument?.ApplyTo(tt);
    return tt;
  }
}
