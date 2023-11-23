using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.CostCentres.List;

public record ListCostCentresQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<CostCentreDTO>>>;
