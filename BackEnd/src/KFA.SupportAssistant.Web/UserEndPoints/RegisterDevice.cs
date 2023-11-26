using System.Configuration;
using System.Security.Claims;
using FastEndpoints.Security;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Contributors.Create;
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
    Post(RegisterDeviceRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new RegisterDeviceRequest { DeviceCode = "Device Code", Description = "Device Description" };
    });
  }

  public override async Task HandleAsync(
    RegisterDeviceRequest request,
    CancellationToken cancellationToken)
  {
    var dto = new DataDeviceDTO { DeviceCode = request.DeviceCode, DateInserted___ = DateTime.UtcNow, DateUpdated___ = DateTime.UtcNow, DeviceCaption = request.DeviceCaption, DeviceName = request.DeviceName, DeviceNumber = null, DeviceRight = request.DeviceRight, Id = null, StationID = request.StationID, TypeOfDevice = request.TypeOfDevice };
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
      await SendAsync(new RegisterDeviceResponse(device.TypeOfDevice, device.StationID, device.DeviceRight, device.DeviceNumber, device.DeviceName, device.DeviceCode, device.DeviceCaption));
    }
    else await SendErrorsAsync(statusCode: 500, cancellationToken);
  }
}
