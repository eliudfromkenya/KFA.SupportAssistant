using Ardalis.Result;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.EndPoints.CostCentres;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.UseCases.Models.Update;

public class PatchCostCentre(IMediator _mediator) : Endpoint<PatchRequest>
{
  public override void Configure()
  {
    Patch(PatchRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(PatchRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Id))
    {
      AddError(request => request.Id ?? "Id", "Id of item to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }
    CostCentreDTO patchFunc(CostCentreDTO tt) => PatchUpdater.Patch< CostCentreDTO,CostCentre>(request.PatchDocument, tt);
    var result = await _mediator.Send(new PatchModelCommand<CostCentreDTO, CostCentre>(request.Id ?? "", patchFunc), cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the cost centre to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = result.Value;
    if (result.IsSuccess)
    {
      Response = new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix, value?.DateInserted___, value?.DateUpdated___);
    }
  }
}
