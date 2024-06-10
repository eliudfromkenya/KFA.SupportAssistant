using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerRemoteAddressRequest, ComputerRemoteAddressRecord>
{
  private const string EndPointId = "ENP-1H6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerRemoteAddressRequest.Route));
    //RequestBinder(new PatchBinder<ComputerRemoteAddressDTO, ComputerRemoteAddress, PatchComputerRemoteAddressRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Remote Address End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer remote address";
      s.Description = "Used to update part of an existing computer remote address. A valid existing computer remote address is required.";
      s.ResponseExamples[200] = new ComputerRemoteAddressRecord("1000", "AnyDesk Number", "Anydesk Password", "", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Team Viewer Address", "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerRemoteAddressRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AnyDeskId))
    {
      AddError(request => request.AnyDeskId, "The computer remote address of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerRemoteAddressDTO patchFunc(ComputerRemoteAddressDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerRemoteAddressDTO, ComputerRemoteAddress>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerRemoteAddressDTO, ComputerRemoteAddress>(CreateEndPointUser.GetEndPointUser(User), request.AnyDeskId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer remote address to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerRemoteAddressRecord(obj.Id, obj.AnyDeskNumber, obj.AnydeskPassword, obj.AssetDetailID, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.TeamViewerAddress, obj.Type, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
