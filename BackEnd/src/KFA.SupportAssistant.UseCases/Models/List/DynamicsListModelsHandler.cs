using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.ContributorAggregate.Specifications;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.UseCases.Models.List;

public class DynamicsListModelsHandler<T, X>(IDbQuery<X> dbQuery)
  : IQueryHandler<DynamicsListModelsQuery<T, X>, Result<string>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<string>> Handle(DynamicsListModelsQuery<T, X> request, CancellationToken cancellationToken)
  {
    var items = await DynamicParam<X>.GetQuery(request.user, dbQuery, request.param, cancellationToken);
    return Result<string>.Success(JsonConvert.SerializeObject(items));
  }
}
