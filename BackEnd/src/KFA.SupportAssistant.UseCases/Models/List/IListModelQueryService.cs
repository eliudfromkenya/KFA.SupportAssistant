using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.Models.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListModelsQueryService<T,X> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  Task<IList<T>> ListAsync();
}
