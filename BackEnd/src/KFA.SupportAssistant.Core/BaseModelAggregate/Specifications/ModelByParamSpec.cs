using Ardalis.Specification;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Specifications;

public class ModelByParamSpec<T> : Specification<T> where T : BaseModel, new()
{
  public ModelByParamSpec(ListParam param)
  {
    Query.Skip(param.Skip ?? 0).Take(param.Take ?? 1000);
  }
}
