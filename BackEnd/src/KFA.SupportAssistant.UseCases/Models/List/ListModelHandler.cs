using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.ContributorAggregate.Specifications;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.UseCases.Models.List;

public class ListModelsHandler<T, X>(IDbQuery<X> dbQuery)
  : IQueryHandler<ListModelsQuery<T, X>, Result<List<T>>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<List<T>>> Handle(ListModelsQuery<T, X> request, CancellationToken cancellationToken)
  {
    var query = DynamicParam<X>.GetQuery(request.user, dbQuery, request.param);
    List<X> objs = [];
    if(query != null)
      objs = await query!.ToListAsync(cancellationToken);
    var resukt = JsonConvert.SerializeObject(objs);
   // var bb = objs.Select(v => (T)v.ToBaseDTO()).ToList();
   // return Result<List<T>>.Success(JsonConvert.DeserializeObject<List<T>>(resukt));
   return Result<List<T>>.Success(objs.Select(v => (T)v.ToBaseDTO()).ToList());
  }
}
