
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

/// <summary>
/// Get a computer remote address by anydesk id.
/// </summary>
/// <remarks>
/// Takes anydesk id and returns a matching computer remote address record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerRemoteAddressByIdRequest, ComputerRemoteAddressRecord>
{
  private const string EndPointId = "ENP-1H4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerRemoteAddressByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Remote Address End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer remote address by specified anydesk id";
      s.Description = "This endpoint is used to retrieve computer remote address with the provided anydesk id";
      s.ExampleRequest = new GetComputerRemoteAddressByIdRequest { AnyDeskId = "anydesk id to retrieve" };
      s.ResponseExamples[200] = new ComputerRemoteAddressRecord("1000", "AnyDesk Number", "Anydesk Password", "", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Team Viewer Address", "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerRemoteAddressByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AnyDeskId))
    {
      AddError(request => request.AnyDeskId, "The anydesk id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerRemoteAddressDTO, ComputerRemoteAddress>(CreateEndPointUser.GetEndPointUser(User), request.AnyDeskId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerRemoteAddressRecord(obj.Id, obj.AnyDeskNumber, obj.AnydeskPassword, obj.AssetDetailID, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.TeamViewerAddress, obj.Type, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
