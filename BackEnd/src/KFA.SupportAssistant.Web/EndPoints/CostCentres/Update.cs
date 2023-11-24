using Ardalis.Result;
using FastEndpoints;
using KFA.SupportAssistant.UseCases.DTOs;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Update an existing CostCentre.
/// </summary>
/// <remarks>
/// Update an existing CostCentre by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update : Endpoint<UpdateCostCentreRequest, UpdateCostCentreResponse>
{
  private readonly IMediator _mediator;

  public Update(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Put(UpdateCostCentreRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    UpdateCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Id))
    {
      AddError(request => request.Id ?? "Id", "Id of item to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<CostCentreDTO>(request.Id ?? "");
    var resultObj = await _mediator.Send(command, cancellationToken);

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the cost centre to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

      var value = request.Adapt(resultObj.Value);
      var result = await _mediator.Send(new UpdateModelCommand<CostCentreDTO>(request.Id ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {      
      Response = new UpdateCostCentreResponse(new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix, value?.DateInserted___, value?.DateUpdated___));
      return;
    }
  }
}

