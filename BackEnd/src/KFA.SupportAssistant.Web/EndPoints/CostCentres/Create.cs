using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Create a new CostCentre
/// </summary>
/// <remarks>
/// Creates a new cost centre given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateCostCentreRequest, CreateCostCentreResponse>
{
  private const string EndPointId = "ENP-151";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateCostCentreRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Cost Centre End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new cost centre";
      s.Description = "This endpoint is used to create a new  cost centre. Here details of cost centre to be created is provided";
      s.ExampleRequest = new CreateCostCentreRequest { CostCentreCode = "1000", Description = "Description", Narration = "Narration", Region = "Region", SupplierCodePrefix = "Supplier Code Prefix" };
      s.ResponseExamples[200] = new CreateCostCentreResponse("1000", "Description", "Narration", "Region", "Supplier Code Prefix", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<CostCentreDTO>();
    requestDTO.Id = request.CostCentreCode;

    var result = await mediator.Send(new CreateModelCommand<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is CostCentreDTO obj)
      {
        Response = new CreateCostCentreResponse(obj.Id, obj.Description, obj.Narration, obj.Region, obj.SupplierCodePrefix, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
