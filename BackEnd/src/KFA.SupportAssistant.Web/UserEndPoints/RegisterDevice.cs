using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Infrastructure.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.UserEndPoints;

/// <summary>
/// Create a new Contributor
/// </summary>
/// <remarks>
/// Creates a new Contributor given a name.
/// </remarks>
public class RegisterDevice(IMediator mediator) : Endpoint<RegisterDeviceRequest, RegisterDeviceResponse>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(RegisterDeviceRequest.Route));
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new RegisterDeviceRequest { DeviceCode = "Device Code", DeviceName="Name" ,Description = "Device Description" };
    });
  }

  public override async Task HandleAsync(
    RegisterDeviceRequest request,
    CancellationToken cancellationToken)
  {
       var dto = new DataDeviceDTO { DeviceCode = request.DeviceCode, DateInserted___ = DateTime.UtcNow, DateUpdated___ = DateTime.UtcNow, DeviceCaption = request.Description, DeviceName = request.DeviceName, DeviceNumber = Functions.GetRandomString(3), DeviceRight = request.DeviceRight, Id = null, StationID = request.StationID, TypeOfDevice = request.TypeOfDevice };
    var command = new UserRegisterDeviceCommand(dto);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }
    if (result.IsSuccess)
    {
      var device = result.Value;
      await SendAsync(new RegisterDeviceResponse(device.TypeOfDevice, device.StationID, device.DeviceRight, device.DeviceNumber, device.DeviceName, device.DeviceCode, device.DeviceCaption, device.Id!));
    }
    else await SendErrorsAsync(statusCode: 500, cancellationToken);
  }
}
