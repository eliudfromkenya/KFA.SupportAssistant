using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.UseCases.Models.Delete;

public record DeleteModelCommand<T>(EndPointUser user, params string[] id) : ICommand<Result> where T : BaseModel, new();
