using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

/// <summary>
/// Create a new ComputerAnydesk
/// </summary>
/// <remarks>
/// Creates a new computer anydesk given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAnydeskRequest, CreateComputerAnydeskResponse>
{
  private const string EndPointId = "ENP-141";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAnydeskRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Anydesk End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer anydesk";
      s.Description = "This endpoint is used to create a new  computer anydesk. Here details of computer anydesk to be created is provided";
      s.ExampleRequest = new CreateComputerAnydeskRequest { AnyDeskId = "1000", AnyDeskNumber = "AnyDesk Number", CostCentreCode = "Cost Centre Code", DeviceName = "Device Name", NameOfUser = "Name Of User", Narration = "Narration", Password = "Password", Type = "Type" };
      s.ResponseExamples[200] = new CreateComputerAnydeskResponse("1000", "AnyDesk Number", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Password", Core.DataLayer.Types.AnyDeskComputerType.Sales, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAnydeskRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAnydeskDTO>();
    requestDTO.Id = request.AnyDeskId;

    var result = await mediator.Send(new CreateModelCommand<ComputerAnydeskDTO, ComputerAnydesk>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAnydeskDTO obj)
      {
        Response = new CreateComputerAnydeskResponse(obj.Id, obj.AnyDeskNumber, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.Password, obj.Type, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
