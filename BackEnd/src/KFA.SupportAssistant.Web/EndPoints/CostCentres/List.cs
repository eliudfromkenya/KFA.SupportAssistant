using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// List all CostCentres
/// </summary>
/// <remarks>
/// List all CostCentres - returns a CostCentreListResponse containing the CostCentres.
/// </remarks>
public class List(IMediator mediator) : Endpoint<ListParam, CostCentreListResponse>
{
  const string Route = "/cost_centres";
  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Description(x => x.WithName("Get Cost Centres"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Retrieves list of cost centres as specified";
      s.Description = "Returns all cost centres within specified range";
      s.ResponseExamples[200] = new CostCentreListResponse { CostCentres = [] };
      s.ExampleRequest = new ListParam { Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      ThrowIfAnyErrors();
    }

    if (result.IsSuccess)
    {
      Response = new CostCentreListResponse
      {
        CostCentres = result.Value.Select(c => new CostCentreRecord(c?.Id, c?.Description, c?.Narration, c?.Region, c?.SupplierCodePrefix,c?.IsActive, c?.DateInserted___, c?.DateUpdated___)).ToList()
      };
    }
  }
}
