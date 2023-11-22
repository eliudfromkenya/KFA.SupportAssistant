using Ardalis.Specification;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Specifications;

public class ModelByQuerySpec<T> : Specification<T> where T : BaseModel, new()
{
  public ModelByQuerySpec(Func<T, bool> func)
  {
    Query.Where(c => func(c));
  }
}
