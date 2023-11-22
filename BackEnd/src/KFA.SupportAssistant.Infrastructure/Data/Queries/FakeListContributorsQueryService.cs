using KFA.SupportAssistant.UseCases.Contributors;
using KFA.SupportAssistant.UseCases.Contributors.List;

namespace KFA.SupportAssistant.Infrastructure.Data.Queries;

public class FakeListContributorsQueryService : IListContributorsQueryService
{
  public Task<IEnumerable<ContributorDTO>> ListAsync()
  {
    List<ContributorDTO> result =
        [new ContributorDTO("AAA-01", "Fake Contributor 1"),
          new ContributorDTO("AAB-01", "Fake Contributor 2")];

    return Task.FromResult(result.AsEnumerable());
  }
}
