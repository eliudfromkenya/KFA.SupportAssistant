using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

/// <summary>
/// Create a new ComputerRemoteAddress
/// </summary>
/// <remarks>
/// Creates a new computer remote address given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerRemoteAddressRequest, CreateComputerRemoteAddressResponse>
{
  private const string EndPointId = "ENP-1H1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerRemoteAddressRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Remote Address End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer remote address";
      s.Description = "This endpoint is used to create a new  computer remote address. Here details of computer remote address to be created is provided";
      s.ExampleRequest = new CreateComputerRemoteAddressRequest { AnyDeskId = "1000", AnyDeskNumber = "AnyDesk Number", AnydeskPassword = "Anydesk Password", AssetDetailID = "", CostCentreCode = "Cost Centre Code", DeviceName = "Device Name", NameOfUser = "Name Of User", Narration = "Narration", TeamViewerAddress = "Team Viewer Address", Type = "Type" };
      s.ResponseExamples[200] = new CreateComputerRemoteAddressResponse("1000", "AnyDesk Number", "Anydesk Password", "", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Team Viewer Address", "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerRemoteAddressRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerRemoteAddressDTO>();
    requestDTO.Id = request.AnyDeskId;

    var result = await mediator.Send(new CreateModelCommand<ComputerRemoteAddressDTO, ComputerRemoteAddress>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerRemoteAddressDTO obj)
      {
        Response = new CreateComputerRemoteAddressResponse(obj.Id, obj.AnyDeskNumber, obj.AnydeskPassword, obj.AssetDetailID, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.TeamViewerAddress, obj.Type, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
