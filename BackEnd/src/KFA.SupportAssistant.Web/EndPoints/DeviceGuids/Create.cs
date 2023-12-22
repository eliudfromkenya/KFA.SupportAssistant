using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// Create a new DeviceGuid
/// </summary>
/// <remarks>
/// Creates a new device guid given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateDeviceGuidRequest, CreateDeviceGuidResponse>
{
  private const string EndPointId = "ENP-191";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateDeviceGuidRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Device Guid End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new device guid";
      s.Description = "This endpoint is used to create a new  device guid. Here details of device guid to be created is provided";
      s.ExampleRequest = new CreateDeviceGuidRequest { Guid = "1000" };
      s.ResponseExamples[200] = new CreateDeviceGuidResponse("1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateDeviceGuidRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<DeviceGuidDTO>();
    requestDTO.Id = request.Guid;

    var result = await mediator.Send(new CreateModelCommand<DeviceGuidDTO, DeviceGuid>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is DeviceGuidDTO obj)
      {
        Response = new CreateDeviceGuidResponse(obj.Id, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
