using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.DTOs;
using Mapster;

namespace KFA.SupportAssistant.UseCases.Models.Update;

public class UpdateModelHandler<T,X>(IRepository<X> _repository)
  : ICommandHandler<UpdateModelCommand<T,X>, Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<T>> Handle(UpdateModelCommand<T,X> request, CancellationToken cancellationToken)
  {
    var id = request?.model?.Id;
    if (string.IsNullOrWhiteSpace(id))
      return Result.Invalid(new ValidationError(nameof(id), "Id of the item to update is required", "404", ValidationSeverity.Error));
    if (await _repository.GetByIdAsync(id, cancellationToken) is not X model)
    {
      return Result.NotFound();
    }
    request?.model?.Adapt(model);
    await _repository.UpdateAsync(model, cancellationToken);

    var ans = model as T;
    if(ans == null)
    {
      ans = request?.model;
      model.Adapt(ans);
    }
    if(ans != null)
    return Result.Success(ans);
    return Result.Error("Unable to convert entity to dto after insert");
  }
}
