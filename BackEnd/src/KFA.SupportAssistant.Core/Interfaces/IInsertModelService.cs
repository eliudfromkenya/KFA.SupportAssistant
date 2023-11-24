using Ardalis.Result;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Interfaces;

public interface IInsertModelService<T> where T : BaseModel
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public Task<Result<T[]>> InsertModel(CancellationToken cancellationToken, params T[] models);
}
