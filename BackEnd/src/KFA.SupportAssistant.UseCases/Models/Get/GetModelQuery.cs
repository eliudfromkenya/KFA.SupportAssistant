using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.Models.Get;

public record GetModelQuery<T,X>(string id) : IQuery<Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new();
