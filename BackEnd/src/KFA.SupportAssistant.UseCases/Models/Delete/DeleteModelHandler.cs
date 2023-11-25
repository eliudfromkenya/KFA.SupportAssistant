using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.Interfaces;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.UseCases.Models.Delete;

public class DeleteModelHandler<T>(IDeleteModelService<T> _deleteCostCentreService)
  : ICommandHandler<DeleteModelCommand<T>, Result> where T : BaseModel, new()
{
  public async Task<Result> Handle(DeleteModelCommand<T> request, CancellationToken cancellationToken)
  {
    // This Approach: Keep Domain Events in the Domain Model / Core project; this becomes a pass-through
    // This is @ardalis's preferred approach
    var result = await _deleteCostCentreService.DeleteModel(cancellationToken, request.id);
  
    return result;


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
