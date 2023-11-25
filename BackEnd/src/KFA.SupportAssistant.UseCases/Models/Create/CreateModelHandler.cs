using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.Interfaces;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Globals.DataLayer;

namespace KFA.SupportAssistant.UseCases.Models.Create;

public class CreateModelHandler<T,X>(IInsertModelService<X> _addService)
  : ICommandHandler<CreateModelCommand<T, X>, Result<T?[]>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<T?[]>> Handle(CreateModelCommand<T, X> request,
    CancellationToken cancellationToken)
  {
    X[] objs = request?.Models?
      .Select(c => c.ToModel())?
      .Where(m => m != null)
      .Select(n => n!)
      .ToArray() ?? [];

    foreach (var obj in objs)
    {
      obj.___DateInserted___ = DateTime.Now.FromDateTime();
      obj.___DateUpdated___ = DateTime.Now.FromDateTime();
      if (string.IsNullOrWhiteSpace(obj.Id))
        obj.Id = Declarations.IdGenerator?.GetNextId<X>();
    }
    var createdItem = await _addService.InsertModel(cancellationToken, objs);
    return createdItem.Value?.Select(c => (T)c.ToBaseDTO())?.ToArray() ?? [];
  }
}
