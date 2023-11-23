using Ardalis.Result;
using Ardalis.SharedKernel;

namespace KFA.SupportAssistant.UseCases.CostCentres.Delete;

public record DeleteCostCentreCommand(string CostCentreId) : ICommand<Result>;
