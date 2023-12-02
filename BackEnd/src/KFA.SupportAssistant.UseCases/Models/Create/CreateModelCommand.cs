using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.UseCases.Models.Create;

/// <summary>
/// Create a new CostCentre.
/// </summary>
/// <param name="Name"></param>
public record CreateModelCommand<T, X>(EndPointUser user, params T[] models) : Ardalis.SharedKernel.ICommand<Result<T?[]>> where T : BaseDTO<X>, new() where X : BaseModel, new();
