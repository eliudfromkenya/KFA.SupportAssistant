using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

/// <summary>
/// Update an existing computer remote address.
/// </summary>
/// <remarks>
/// Update an existing computer remote address by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerRemoteAddressRequest, UpdateComputerRemoteAddressResponse>
{
  private const string EndPointId = "ENP-1H7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerRemoteAddressRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Remote Address End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Remote Address";
      s.Description = "This endpoint is used to update  computer remote address, making a full replacement of computer remote address with a specifed valuse. A valid computer remote address is required.";
      s.ExampleRequest = new UpdateComputerRemoteAddressRequest { AnyDeskId = "1000", AnyDeskNumber = "AnyDesk Number", AnydeskPassword = "Anydesk Password", AssetDetailID = "", CostCentreCode = "Cost Centre Code", DeviceName = "Device Name", NameOfUser = "Name Of User", Narration = "Narration", TeamViewerAddress = "Team Viewer Address", Type = "Type" };
      s.ResponseExamples[200] = new UpdateComputerRemoteAddressResponse(new ComputerRemoteAddressRecord("1000", "AnyDesk Number", "Anydesk Password", "", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Team Viewer Address", "Type", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerRemoteAddressRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AnyDeskId))
    {
      AddError(request => request.AnyDeskId, "The anydesk id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerRemoteAddressDTO, ComputerRemoteAddress>(CreateEndPointUser.GetEndPointUser(User), request.AnyDeskId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer remote address to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerRemoteAddressDTO, ComputerRemoteAddress>(CreateEndPointUser.GetEndPointUser(User), request.AnyDeskId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerRemoteAddressResponse(new ComputerRemoteAddressRecord(obj.Id, obj.AnyDeskNumber, obj.AnydeskPassword, obj.AssetDetailID, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.TeamViewerAddress, obj.Type, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
