using KFA.SupportAssistant.Globals;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.Services;

public static class PatchUpdater
{
  internal static T Patch<T, X>(JsonPatchDocument<T> patchDocument, T tt) where T : BaseDTO<X>, new() where X : BaseModel, new()
  {
    patchDocument.ApplyTo(tt);
    return tt;
  }
}
