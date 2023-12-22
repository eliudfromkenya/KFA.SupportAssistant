using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// Create a new DataDevice
/// </summary>
/// <remarks>
/// Creates a new data device given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateDataDeviceRequest, CreateDataDeviceResponse>
{
  private const string EndPointId = "ENP-171";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateDataDeviceRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Data Device End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new data device";
      s.Description = "This endpoint is used to create a new  data device. Here details of data device to be created is provided";
      s.ExampleRequest = new CreateDataDeviceRequest { DeviceCaption = "Device Caption", DeviceCode = "Device Code", DeviceId = "1000", DeviceName = "Device Name", DeviceNumber = "Device Number", DeviceRight = "Device Right", StationID = "Station", TypeOfDevice = "Type Of Device" };
      s.ResponseExamples[200] = new CreateDataDeviceResponse("Device Caption", "Device Code", "1000", "Device Name", "Device Number", "Device Right", "Station", "Type Of Device", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateDataDeviceRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<DataDeviceDTO>();
    requestDTO.Id = request.DeviceId;

    var result = await mediator.Send(new CreateModelCommand<DataDeviceDTO, DataDevice>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is DataDeviceDTO obj)
      {
        Response = new CreateDataDeviceResponse(obj.DeviceCaption, obj.DeviceCode, obj.Id, obj.DeviceName, obj.DeviceNumber, obj.DeviceRight, obj.StationID, obj.TypeOfDevice, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
