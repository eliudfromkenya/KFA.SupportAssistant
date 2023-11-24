using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant.Core.Interfaces;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.UseCases.Models.Create;

public class CreateModelHandler<T,X>(IInsertModelService<X> _addService)
  : ICommandHandler<CreateModelCommand<T, X>, Result<string[]>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<string[]>> Handle(CreateModelCommand<T, X> request,
    CancellationToken cancellationToken)
  {
    X[] objs = request?.Models?
      .Select(c => c.ToModel())?
      .Where(m => m != null)
      .Select(n => n!)
      .ToArray() ?? [];
    var createdItem = await _addService.InsertModel(cancellationToken, objs);
    return createdItem.Value?.Select(c => c.Id)?.ToArray() ?? [];
  }
}
