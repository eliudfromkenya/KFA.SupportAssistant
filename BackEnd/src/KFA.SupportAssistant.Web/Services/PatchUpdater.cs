using KFA.SupportAssistant.Globals;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.Services;

public static class PatchUpdater
{
  internal static X Patch<T,X>(JsonPatchDocument<T> patchDocument, X tt) where T : BaseDTO<X>, new() where X : BaseModel, new()
  {
    if (tt.ToBaseDTO() is T obj)
    {
      patchDocument.ApplyTo(obj);
      return (X)obj.ToModel()!;
    }
    return tt;
  }
}
