using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.CostCentres.Get;

public record GetCostCentreQuery(string CostCentreId) : IQuery<Result<CostCentreDTO>>;
