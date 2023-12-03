using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Create a new CostCentre
/// </summary>
/// <remarks>
/// Creates a new CostCentre given a name.
/// </remarks>
public class Create(IMediator mediator) : Endpoint<CreateCostCentreRequest, CreateCostCentreResponse>
{
  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateCostCentreRequest.Route));
    Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Description(x => x.WithName("Add Cost Centre"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "User to create a new cost centre";
      s.Description = "Cost centre to be created details are provided here";
      s.ExampleRequest = new CreateCostCentreRequest { CostCentreCode = "1000", Description = "Cost Centre Name" };
      s.ResponseExamples[200] = new CreateCostCentreResponse("1100", true, "Cost Centre Name", "Narration", "Region", "S3A", DateTime.UtcNow, DateTime.UtcNow);
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
      ThrowIfAnyErrors();
    }

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is CostCentreDTO obj)
      {
        Response = new CreateCostCentreResponse(obj.Id,obj.IsActive, obj.Description!, obj.Narration, obj.Region, obj.SupplierCodePrefix, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
