using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.Web.Binders;

public class GetParamBinder : IRequestBinder<ListParam>
{
  public ValueTask<ListParam> BindAsync(BinderContext ctx, CancellationToken ct)
  {
    return ValueTask.FromResult(new ListParam()
    {
      Skip = int.TryParse(ctx.HttpContext.Request.Query["Skip"], out int skip)? skip:0,
      Take = int.TryParse(ctx.HttpContext.Request.Query["Take"], out int take) ? take : 0,
      Param = ctx.HttpContext.Request.Query["Param"]
    });
  }
}
