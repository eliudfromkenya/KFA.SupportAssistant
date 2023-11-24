using Ardalis.Result;
using FastEndpoints;
using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Delete a CostCentre.
/// </summary>
/// <remarks>
/// Delete a CostCentre by providing a valid integer id.
/// </remarks>
public class Delete : Endpoint<DeleteCostCentreRequest>
{
  private readonly IMediator _mediator;

  public Delete(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Delete(DeleteCostCentreRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    DeleteCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Id))
    {
      AddError(request => request.Id ?? "Id", "Item to be deleted is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new DeleteModelCommand<CostCentre>(request.Id ?? "");

    var result = await _mediator.Send(command, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    };
  }
}
