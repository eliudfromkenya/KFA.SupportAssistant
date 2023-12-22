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

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// Update an existing data device.
/// </summary>
/// <remarks>
/// Update an existing data device by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateDataDeviceRequest, UpdateDataDeviceResponse>
{
  private const string EndPointId = "ENP-177";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateDataDeviceRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Data Device End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Data Device";
      s.Description = "This endpoint is used to update  data device, making a full replacement of data device with a specifed valuse. A valid data device is required.";
      s.ExampleRequest = new UpdateDataDeviceRequest { DeviceCaption = "Device Caption", DeviceCode = "Device Code", DeviceId = "1000", DeviceName = "Device Name", DeviceNumber = "Device Number", DeviceRight = "Device Right", StationID = "Station", TypeOfDevice = "Type Of Device" };
      s.ResponseExamples[200] = new UpdateDataDeviceResponse(new DataDeviceRecord("Device Caption", "Device Code", "1000", "Device Name", "Device Number", "Device Right", "Station", "Type Of Device", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateDataDeviceRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.DeviceId))
    {
      AddError(request => request.DeviceId, "The device id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<DataDeviceDTO, DataDevice>(CreateEndPointUser.GetEndPointUser(User), request.DeviceId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the data device to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<DataDeviceDTO, DataDevice>(CreateEndPointUser.GetEndPointUser(User), request.DeviceId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateDataDeviceResponse(new DataDeviceRecord(obj.DeviceCaption, obj.DeviceCode, obj.Id, obj.DeviceName, obj.DeviceNumber, obj.DeviceRight, obj.StationID, obj.TypeOfDevice, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
