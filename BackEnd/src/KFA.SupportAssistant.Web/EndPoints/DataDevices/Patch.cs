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

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchDataDeviceRequest, DataDeviceRecord>
{
  private const string EndPointId = "ENP-176";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchDataDeviceRequest.Route));
    //RequestBinder(new PatchBinder<DataDeviceDTO, DataDevice, PatchDataDeviceRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Data Device End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a data device";
      s.Description = "Used to update part of an existing data device. A valid existing data device is required.";
      s.ResponseExamples[200] = new DataDeviceRecord("Device Caption", "Device Code", "1000", "Device Name", "Device Number", "Device Right", "Station", "Type Of Device", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchDataDeviceRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.DeviceId))
    {
      AddError(request => request.DeviceId, "The data device of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    DataDeviceDTO patchFunc(DataDeviceDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<DataDeviceDTO, DataDevice>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<DataDeviceDTO, DataDevice>(CreateEndPointUser.GetEndPointUser(User), request.DeviceId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the data device to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new DataDeviceRecord(obj.DeviceCaption, obj.DeviceCode, obj.Id, obj.DeviceName, obj.DeviceNumber, obj.DeviceRight, obj.StationID, obj.TypeOfDevice, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
