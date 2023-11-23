using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant.Core.Interfaces;

namespace KFA.SupportAssistant.UseCases.CostCentres.Create;

public class CreateCostCentreHandler(IInsertModelService<CostCentre> _addService)
  : ICommandHandler<CreateCostCentreCommand, Result<string>>
{
  public async Task<Result<string>> Handle(CreateCostCentreCommand request,
    CancellationToken cancellationToken)
  {
    var newCostCentre = new CostCentre { Description = request.Description, Id = request.Id, SupplierCodePrefix = request.SupplierPrefix, Narration = request.Narration, Region = request.Region };
    var createdItem = await _addService.InsertModel(cancellationToken, newCostCentre);
    return createdItem.Value?.Id ?? string.Empty;
  }
}
