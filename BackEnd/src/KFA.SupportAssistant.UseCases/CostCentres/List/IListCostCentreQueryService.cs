using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.CostCentres.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListCostCentresQueryService
{
  Task<IEnumerable<CostCentreDTO>> ListAsync();
}
