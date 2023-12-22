using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// Get a data device by device id.
/// </summary>
/// <remarks>
/// Takes device id and returns a matching data device record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetDataDeviceByIdRequest, DataDeviceRecord>
{
  private const string EndPointId = "ENP-174";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetDataDeviceByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Data Device End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets data device by specified device id";
      s.Description = "This endpoint is used to retrieve data device with the provided device id";
      s.ExampleRequest = new GetDataDeviceByIdRequest { DeviceId = "device id to retrieve" };
      s.ResponseExamples[200] = new DataDeviceRecord("Device Caption", "Device Code", "1000", "Device Name", "Device Number", "Device Right", "Station", "Type Of Device", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetDataDeviceByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.DeviceId))
    {
      AddError(request => request.DeviceId, "The device id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<DataDeviceDTO, DataDevice>(CreateEndPointUser.GetEndPointUser(User), request.DeviceId ?? "");
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
      Response = new DataDeviceRecord(obj.DeviceCaption, obj.DeviceCode, obj.Id, obj.DeviceName, obj.DeviceNumber, obj.DeviceRight, obj.StationID, obj.TypeOfDevice, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
