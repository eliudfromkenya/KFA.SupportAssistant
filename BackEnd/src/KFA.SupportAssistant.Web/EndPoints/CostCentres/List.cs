using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// List all CostCentres
/// </summary>
/// <remarks>
/// List all CostCentres - returns a CostCentreListResponse containing the CostCentres.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, CostCentreListResponse>
{
  private const string EndPointId = "ENP-015";
  public const string Route = "/cost_centres";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Cost Centres List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Retrieves list of cost centres as specified";
      s.Description = "Returns all cost centres within specified range / condition";
      s.RequestParam(r => r.Take, "overriden username description");
      s.ResponseExamples[200] = new CostCentreListResponse { CostCentres = [] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "SupplierCodePrefix.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Description, SupplierCodePrefix}", Parameters = ["S3", "3100"], OrderByConditions = ["Description", "SupplierCodePrefix"] }), Skip = 0, Take = 1000 };
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
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new CostCentreListResponse
      {
        CostCentres = result.Value.Select(c => new CostCentreRecord(c?.Id, c?.Description, c?.Narration, c?.Region, c?.SupplierCodePrefix, c?.IsActive, c?.DateInserted___, c?.DateUpdated___)).ToList()
      };
    }
  }
}
