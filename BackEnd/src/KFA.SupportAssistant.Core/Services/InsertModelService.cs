using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.BaseModelAggregate.Events;
using KFA.SupportAssistant.Globals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KFA.SupportAssistant.Core.Interfaces;

public class InsertModelService<T>(IRepository<T> _repository,
  IMediator _mediator,
  ILogger<InsertModelService<T>> _logger) : IInsertModelService<T> where T : BaseModel, new()
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public async Task<Result<T[]>> InsertModel(EndPointUser? user, CancellationToken cancellationToken, params T[] models)
  {
    _logger.LogInformation("Inserting model {type} - {length}", typeof(T), models?.Length);
    if (models?.Length < 1)
      return Result.Error("No elements to add are provided");

    T[] objs = [.. await _repository.AddRangeAsync(models!, cancellationToken)];
    var domainEvent = new ModelInsertedEvent<T>(objs);
    await _mediator.Publish(domainEvent, cancellationToken);
    return Result.Success(objs);
  }
}
