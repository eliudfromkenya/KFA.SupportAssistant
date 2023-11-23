using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.CostCentres.Update;

public record UpdateCostCentreCommand(string? Id, string? Description, string? Narration, string? SupplierPrefix, string? Region) : ICommand<Result<CostCentreDTO>>;
