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

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchDeviceGuidRequest, DeviceGuidRecord>
{
  private const string EndPointId = "ENP-196";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchDeviceGuidRequest.Route));
    //RequestBinder(new PatchBinder<DeviceGuidDTO, DeviceGuid, PatchDeviceGuidRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Device Guid End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a device guid";
      s.Description = "Used to update part of an existing device guid. A valid existing device guid is required.";
      s.ResponseExamples[200] = new DeviceGuidRecord("1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchDeviceGuidRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Guid))
    {
      AddError(request => request.Guid, "The device guid of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    DeviceGuidDTO patchFunc(DeviceGuidDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<DeviceGuidDTO, DeviceGuid>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<DeviceGuidDTO, DeviceGuid>(CreateEndPointUser.GetEndPointUser(User), request.Guid ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the device guid to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new DeviceGuidRecord(obj.Id, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
