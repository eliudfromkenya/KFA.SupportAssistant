using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.UseCases.Models;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.Models.List;
public class ListModelsHandler<T,X>(IListModelsQueryService<T,X> _query)
  : IQueryHandler<ListModelsQuery<T,X>, Result<IEnumerable<T>>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<IEnumerable<T>>> Handle(ListModelsQuery<T,X> request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
