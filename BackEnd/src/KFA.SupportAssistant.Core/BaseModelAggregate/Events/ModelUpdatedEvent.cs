using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.BaseModelAggregate.Events;

/// <summary>
/// A domain event that is dispatched whenever a model is deleted.
/// The DeleteModelService is used to dispatch this event.
/// </summary>
internal sealed class ModelUpdatedEvent<T>(string id, T model) : DomainEventBase where T : BaseModel
{
  public string Id { get; init; } = id;
  public T Model { get; init; } = model;
}
