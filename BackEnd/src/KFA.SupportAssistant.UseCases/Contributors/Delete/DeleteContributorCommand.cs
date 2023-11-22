using Ardalis.Result;
using Ardalis.SharedKernel;

namespace KFA.SupportAssistant.UseCases.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
