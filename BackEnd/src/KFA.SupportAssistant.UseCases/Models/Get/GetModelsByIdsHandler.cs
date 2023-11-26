using Ardalis.Result;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using KFA.SupportAssistant.Core.ContributorAggregate.Specifications;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.Models.Get;

namespace KFA.SupportAssistant.UseCases.Xs.Get;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetModelsByIdsHandler<T, X>(IReadRepository<X> _repository)
  : IQueryHandler<GetModelsByIdsQuery<T, X>, Result<T[]>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<T[]>> Handle(GetModelsByIdsQuery<T, X> request, CancellationToken cancellationToken)
  {
    var spec = new ModelByQuerySpec<X>(tt => request?.ids?.Contains(tt.Id) ?? false);
    var entities = await _repository.ListAsync(spec, cancellationToken);
    if (entities?.Count == 0) return Result.NotFound();

    var results = entities?
      .Select(c => ((BaseModel)c is T obj) ? obj : null)?
      .Where(v => v != null)
       .Select(c => c!)
      .ToArray();

    return results?.Length > 0 ? results : (Result<T[]>)Result.Error("Unable to convert the result");
  }
}
