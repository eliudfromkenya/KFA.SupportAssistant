using FastEndpoints;
using KFA.SupportAssistant.UseCases.DTOs;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using Mapster;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// List all CostCentres
/// </summary>
/// <remarks>
/// List all CostCentres - returns a CostCentreListResponse containing the CostCentres.
/// </remarks>
public class List :  Endpoint<ListCostCentreRequest, CostCentreListResponse>
{
  private readonly IMediator _mediator;

  public List(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Get(ListCostCentreRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(ListCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelQuery<CostCentreDTO>(new ListParam { Skip=request.Skip, Take=request.Take });
    var result = await _mediator.Send(command, cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CostCentreListResponse
      {
        CostCentres = result.Value.Select(c => new CostCentreRecord(c?.Id, c?.Description, c?.Narration, c?.Region, c?.SupplierCodePrefix, c?.DateInserted___, c?.DateUpdated___)).ToList()
      };
    }
  }
}
