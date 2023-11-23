using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant.UseCases.DTOs;

namespace KFA.SupportAssistant.UseCases.CostCentres.Update;

public class UpdateCostCentreHandler(IRepository<CostCentre> _repository)
  : ICommandHandler<UpdateCostCentreCommand, Result<CostCentreDTO>>
{
  public async Task<Result<CostCentreDTO>> Handle(UpdateCostCentreCommand request, CancellationToken cancellationToken)
  {
    if(await _repository.GetByIdAsync(request.Id ?? string.Empty, cancellationToken) is not CostCentre costCentre)
    {
      return Result.NotFound();
    }

    CostCentreDTO existingCostCentre = costCentre;
    existingCostCentre.SupplierCodePrefix = request.SupplierPrefix;
    existingCostCentre.Narration = request.Narration;
    existingCostCentre.Description = request.Description;
    existingCostCentre.Region = request.Region;

    await _repository.UpdateAsync(existingCostCentre, cancellationToken);

    return Result.Success(existingCostCentre);
  }
}
