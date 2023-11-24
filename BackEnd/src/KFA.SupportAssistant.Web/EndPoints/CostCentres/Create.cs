using FastEndpoints;
using KFA.SupportAssistant.UseCases.DTOs;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Create a new CostCentre
/// </summary>
/// <remarks>
/// Creates a new CostCentre given a name.
/// </remarks>
public class Create : Endpoint<CreateCostCentreRequest, CreateCostCentreResponse>
{
  private readonly IMediator _mediator;

  public Create(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Post(CreateCostCentreRequest.Route);
    AllowAnonymous();
    //Claims("AdminID", "EmployeeID");
    // Roles("Admin", "Manager","AAA-06");
    Permissions("R-AAA-10", "DeleteUsersPermission");
    //Policy(x => x.RequireAssertion(...));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Create a new CostCentre nnnh.";
      s.Description = "Create a new CostCentre. A valid name is required. hgklgjk";
      s.ExampleRequest = new CreateCostCentreRequest { Description = "CostCentre Name" };
    });
  }

  public override async Task HandleAsync(
    CreateCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<CostCentreDTO>();
    requestDTO.Id = request.CostCentreCode;

    var result = await _mediator.Send(new CreateModelCommand<CostCentreDTO>(requestDTO), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CreateCostCentreResponse(result.Value?.Id, result.Value?.Description!, result.Value?.Narration, result.Value?.Region, result.Value?.SupplierCodePrefix, result.Value?.DateInserted___, result.Value?.DateUpdated___);
      return;
    }
    // TODO: Handle other cases as necessary
  }
}
