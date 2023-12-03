using System.Text;
using KFA.SupportAssistant.Globals;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KFA.SupportAssistant.Web.Services;

public static class PatchUpdater
{
  internal static async Task<T> Patch<T, X>(Func<JsonPatchDocument<T>>  getPatchDocument, HttpContext httpContext,string body, T tt, CancellationToken cancellationToken) where T : BaseDTO<X>, new() where X : BaseModel, new()
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
        await Task.Run(() =>
        {
          try
          {
            tt.UpdateFromDictionary(values);
          }
          catch (Exception ex)
          {
            throw new Exception("Unable to understand request body of patch method, " + ex.Message);
          }
        }, cancellationToken);
        //patchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<T>>(vals);
      }
     }
    patchDocument?.ApplyTo(tt);
    return tt;
  }
}
