using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.UseCases.Models.Patch;

public record PatchModelCommand<T, X>(EndPointUser user, string id, Func<T,T> applyChanges) : ICommand<Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new();
