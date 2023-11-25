using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.UseCases.Models.Update;

public record UpdateModelCommand<T,X>(string id, T? model) : ICommand<Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new();
