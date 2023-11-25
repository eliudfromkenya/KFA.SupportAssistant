using FastEndpoints;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
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
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Create a new CostCentre nnnh.";
      s.Description = "Create a new CostCentre. A valid name is required. hgklgjk";
      s.ExampleRequest = new ListCostCentreRequest { Skip = 0, Take = 10};
    });
  }

  public override async Task HandleAsync(ListCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<CostCentreDTO, CostCentre>(new ListParam { Skip=request.Skip, Take=request.Take });
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new CostCentreListResponse
      {
        CostCentres = result.Value.Select(c => new CostCentreRecord(c?.Id, c?.Description, c?.Narration, c?.Region, c?.SupplierCodePrefix, c?.DateInserted___, c?.DateUpdated___)).ToList()
      };
    }
  }
}
