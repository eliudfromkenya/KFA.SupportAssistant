using System.Text;
using System.Text.Json;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;
using Microsoft.AspNetCore.JsonPatch;

namespace KFA.SupportAssistant.Web.Binders;

public class PatchBinder<T, X, Y> : IRequestBinder<Y> where T : BaseDTO<X>, new() where Y : JsonPatchDocument<T>, IPlainTextRequest, new() where X : BaseModel, new()
{
  public ValueTask<Y> BindAsync(BinderContext ctx, CancellationToken ct)
  {
    // populate and return a request dto object however you please...
    //return new Y
    //{
    //  Id = ctx.HttpContext.Request.RouteValues["id"]?.ToString()!,
    //  CustomerID = ctx.HttpContext.Request.Headers["CustomerID"].ToString()!,
    //  Product = await JsonSerializer.DeserializeAsync<Product>(
    //      ctx.HttpContext.Request.Body,
    //      new JsonSerializerOptions(),
    //      ct)
    //};

    Y mx = new();
    using StreamReader reader
                  = new StreamReader(ctx.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true);
    var bodyStr = reader.ReadToEnd();

    mx.Content = bodyStr;
    var req = JsonSerializer.DeserializeAsync<Y>(
       ctx.HttpContext.Request.Body, ctx.SerializerOptions, ct);

    return ValueTask.FromResult(mx);
  }
}

