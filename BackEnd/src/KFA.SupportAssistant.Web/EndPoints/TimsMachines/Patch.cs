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

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchTimsMachineRequest, TimsMachineRecord>
{
  private const string EndPointId = "ENP-226";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchTimsMachineRequest.Route));
    //RequestBinder(new PatchBinder<TimsMachineDTO, TimsMachine, PatchTimsMachineRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Tims Machine End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a tims machine";
      s.Description = "Used to update part of an existing tims machine. A valid existing tims machine is required.";
      s.ResponseExamples[200] = new TimsMachineRecord("Class Type", 0, "Domain Name", "External IP Address", "External Port Number", "Internal IP Address", "Internal Port Number", "1000", "Narration", true, "Serial Number", "Tims  Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchTimsMachineRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MachineID))
    {
      AddError(request => request.MachineID, "The tims machine of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    TimsMachineDTO patchFunc(TimsMachineDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<TimsMachineDTO, TimsMachine>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<TimsMachineDTO, TimsMachine>(CreateEndPointUser.GetEndPointUser(User), request.MachineID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the tims machine to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new TimsMachineRecord(obj.ClassType, obj.CurrentStatus, obj.DomainName, obj.ExternalIPAddress, obj.ExternalPortNumber, obj.InternalIPAddress, obj.InternalPortNumber, obj.Id, obj.Narration, obj.ReadyForUse, obj.SerialNumber, obj.TimsName, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
