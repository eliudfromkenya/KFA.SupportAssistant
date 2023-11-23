using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant.Core.ContributorAggregate.Specifications;
using KFA.SupportAssistant.UseCases.DTOs;
namespace KFA.SupportAssistant.UseCases.CostCentres.Get;

/// <summary>
/// Queries don't necessarily need to use repository methods, but they can if it's convenient
/// </summary>
public class GetCostCentreHandler(IReadRepository<CostCentre> _repository)
  : IQueryHandler<GetCostCentreQuery, Result<CostCentreDTO>>
{
  public async Task<Result<CostCentreDTO>> Handle(GetCostCentreQuery request, CancellationToken cancellationToken)
  {
    var spec = new ModelByIdSpec<CostCentre>(request.CostCentreId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return (CostCentreDTO)entity;
  }
}
