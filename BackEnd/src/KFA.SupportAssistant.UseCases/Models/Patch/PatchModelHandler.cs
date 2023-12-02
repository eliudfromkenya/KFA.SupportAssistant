using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.Interfaces;
using KFA.SupportAssistant.Globals;
using Mapster;

namespace KFA.SupportAssistant.UseCases.Models.Patch;

public class PatchModelHandler<T, X>(IRepository<X> _repository, IUpdateModelService<X> _updateService)
  : ICommandHandler<PatchModelCommand<T, X>, Result<T>> where T : BaseDTO<X>, new() where X : BaseModel, new()
{
  public async Task<Result<T>> Handle(PatchModelCommand<T, X> request, CancellationToken cancellationToken)
  {
    var id = request.id;
    if (string.IsNullOrWhiteSpace(id))
      return Result.Invalid(new ValidationError(nameof(id), "Id of the item to patch is required", "404", ValidationSeverity.Error));

    if (await _repository.GetByIdAsync(id, cancellationToken) is not X model)
    {
      return Result.NotFound();
    }

    if (model.ToBaseDTO() is T baseModel)
      request.applyChanges(baseModel).Adapt(model);   

    model.___DateUpdated___ = DateTime.Now.FromDateTime();
    model = await _updateService.UpdateModel(request.user, id, model, cancellationToken);

    if (model.ToBaseDTO() is T ans)
      return Result.Success(ans);
    return Result.Error("Unable to convert entity to dto after insert");
  }
}
