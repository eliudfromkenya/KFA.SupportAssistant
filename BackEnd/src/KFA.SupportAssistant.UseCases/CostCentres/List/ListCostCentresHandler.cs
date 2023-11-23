using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.CostCentres.List;

public class ListCostCentresHandler(IListCostCentresQueryService _query)
  : IQueryHandler<ListCostCentresQuery, Result<IEnumerable<CostCentreDTO>>>
{
  public async Task<Result<IEnumerable<CostCentreDTO>>> Handle(ListCostCentresQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
