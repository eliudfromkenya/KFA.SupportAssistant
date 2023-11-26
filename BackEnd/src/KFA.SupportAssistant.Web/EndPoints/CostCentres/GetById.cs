using Ardalis.Result;
using FastEndpoints;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Get a CostCentre by integer ID.
/// </summary>
/// <remarks>
/// Takes a positive integer ID and returns a matching CostCentre record.
/// </remarks>
public class GetById : Endpoint<GetCostCentreByIdRequest, CostCentreRecord>
{
  private readonly IMediator _mediator;

  public GetById(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get(GetCostCentreByIdRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetCostCentreByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Id))
    {
      AddError(request => request.Id ?? "Id", "Id of item to be retrieved is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<CostCentreDTO, CostCentre>(request.Id ?? "");
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
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
