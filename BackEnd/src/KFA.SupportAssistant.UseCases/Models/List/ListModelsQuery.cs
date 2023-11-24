using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.Models.List;

public record ListModelsQuery<X,T>(int? Skip, int? Take) : IQuery<Result<IEnumerable<T>>> where T : BaseDTO<X>, new() where X : BaseModel, new();
