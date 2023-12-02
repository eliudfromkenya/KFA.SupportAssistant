using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.UseCases.Models.Get;

public record GetModelsByIdsQuery<T, X>(EndPointUser user, params string[] ids) : IQuery<Result<T[]>> where T : BaseDTO<X>, new() where X : BaseModel, new();
