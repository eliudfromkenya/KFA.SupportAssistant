using Ardalis.Specification;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Specifications;

public class ContributorByIdSpec : Specification<Contributor>
{
  public ContributorByIdSpec(string contributorId)
  {
    Query
        .Where(contributor => contributor.Id == contributorId);
  }
}
