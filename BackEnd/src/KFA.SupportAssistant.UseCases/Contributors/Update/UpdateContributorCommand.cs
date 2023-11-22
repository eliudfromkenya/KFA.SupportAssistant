using Ardalis.Result;
using Ardalis.SharedKernel;

namespace KFA.SupportAssistant.UseCases.Contributors.Update;

public record UpdateContributorCommand(string ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
