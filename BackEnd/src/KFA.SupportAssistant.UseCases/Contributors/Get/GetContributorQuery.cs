using Ardalis.Result;
using Ardalis.SharedKernel;

namespace KFA.SupportAssistant.UseCases.Contributors.Get;

public record GetContributorQuery(string ContributorId) : IQuery<Result<ContributorDTO>>;
