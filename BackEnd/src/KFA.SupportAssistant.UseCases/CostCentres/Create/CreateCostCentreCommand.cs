using Ardalis.Result;

namespace KFA.SupportAssistant.UseCases.CostCentres.Create;

/// <summary>
/// Create a new CostCentre.
/// </summary>
/// <param name="Name"></param>
public record CreateCostCentreCommand(string? Id, string? Description, string? Narration, string? SupplierPrefix, string? Region) : Ardalis.SharedKernel.ICommand<Result<string>>;
