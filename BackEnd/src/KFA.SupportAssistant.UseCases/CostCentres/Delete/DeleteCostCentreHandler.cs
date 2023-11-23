using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant.Core.Interfaces;

namespace KFA.SupportAssistant.UseCases.CostCentres.Delete;

public class DeleteCostCentreHandler(IDeleteModelService<CostCentre> _deleteCostCentreService)
  : ICommandHandler<DeleteCostCentreCommand, Result>
{
  public async Task<Result> Handle(DeleteCostCentreCommand request, CancellationToken cancellationToken)
  {
    // This Approach: Keep Domain Events in the Domain Model / Core project; this becomes a pass-through
    // This is @ardalis's preferred approach
    return await _deleteCostCentreService.DeleteModel(cancellationToken, request.CostCentreId);

    // Another Approach: Do the real work here including dispatching domain events - change the event from internal to public
    // @ardalis prefers using the service above so that **domain** event behavior remains in the **domain model** (core project)
    // var aggregateToDelete = await _repository.GetByIdAsync(request.CostCentreId);
    // if (aggregateToDelete == null) return Result.NotFound();

    // await _repository.DeleteAsync(aggregateToDelete);
    // var domainEvent = new CostCentreDeletedEvent(request.CostCentreId);
    // await _mediator.Publish(domainEvent);
    // return Result.Success();
  }
}
