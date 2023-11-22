using Ardalis.Specification;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Specifications;

public class ModelByIdSpec<T> : Specification<T> where T : BaseModel, new()
{
  public ModelByIdSpec(string id)
  {
    Query
        .Where(model => model.Id == id);
  }
}
