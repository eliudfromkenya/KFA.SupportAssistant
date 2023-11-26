using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.UseCases.Models.Delete;

public record DeleteModelCommand<T>(params string[] id) : ICommand<Result> where T : BaseModel, new();
