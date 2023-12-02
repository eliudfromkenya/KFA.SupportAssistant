using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.BaseModelAggregate.Events;
using KFA.SupportAssistant.Core.ContributorAggregate.Specifications;
using KFA.SupportAssistant.Globals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KFA.SupportAssistant.Core.Interfaces;

public class DeleteModelService<T>(IRepository<T> _repository,
  IMediator _mediator,
  ILogger<DeleteModelService<T>> _logger) : IDeleteModelService<T> where T : BaseModel, new()
{
  // This service and method exist to provide a place in which to fire domain events
  // when deleting this aggregate root entity
  public async Task<Result> DeleteModel(EndPointUser? user, CancellationToken cancellationToken, params string[] ids)
  {
    _logger.LogInformation("Deleting model {type} - {ids}", typeof(T), string.Join(",", ids));
    if (ids?.Length < 1)
      return Result.Error("cc");

    var aggregatesToDelete = await _repository.ListAsync(new ModelByIdsSpec<T>(ids!), cancellationToken);
    if (aggregatesToDelete?.Count < 1) return Result.NotFound("No element(s) to delete have been found.");

    await _repository.DeleteRangeAsync(aggregatesToDelete!, cancellationToken);
    var domainEvent = new ModelDeletedEvent<T>([.. aggregatesToDelete]);
    await _mediator.Publish(domainEvent, cancellationToken);
    return Result.Success();
  }
}
