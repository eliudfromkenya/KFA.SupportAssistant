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

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// Update an existing device guid.
/// </summary>
/// <remarks>
/// Update an existing device guid by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateDeviceGuidRequest, UpdateDeviceGuidResponse>
{
  private const string EndPointId = "ENP-197";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateDeviceGuidRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Device Guid End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Device Guid";
      s.Description = "This endpoint is used to update  device guid, making a full replacement of device guid with a specifed valuse. A valid device guid is required.";
      s.ExampleRequest = new UpdateDeviceGuidRequest { Guid = "1000" };
      s.ResponseExamples[200] = new UpdateDeviceGuidResponse(new DeviceGuidRecord("1000", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateDeviceGuidRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Guid))
    {
      AddError(request => request.Guid, "The guid of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<DeviceGuidDTO, DeviceGuid>(CreateEndPointUser.GetEndPointUser(User), request.Guid ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the device guid to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<DeviceGuidDTO, DeviceGuid>(CreateEndPointUser.GetEndPointUser(User), request.Guid ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateDeviceGuidResponse(new DeviceGuidRecord(obj.Id, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
