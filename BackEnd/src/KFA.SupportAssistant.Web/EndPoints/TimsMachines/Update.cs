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

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// Update an existing tims machine.
/// </summary>
/// <remarks>
/// Update an existing tims machine by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateTimsMachineRequest, UpdateTimsMachineResponse>
{
  private const string EndPointId = "ENP-227";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateTimsMachineRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Tims Machine End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Tims Machine";
      s.Description = "This endpoint is used to update  tims machine, making a full replacement of tims machine with a specifed valuse. A valid tims machine is required.";
      s.ExampleRequest = new UpdateTimsMachineRequest { ClassType = "Class Type", CurrentStatus = 0, DomainName = "Domain Name", ExternalIPAddress = "External IP Address", ExternalPortNumber = "External Port Number", InternalIPAddress = "Internal IP Address", InternalPortNumber = "Internal Port Number", MachineID = "1000", Narration = "Narration", ReadyForUse = true, SerialNumber = "Serial Number", TimsName = "Tims  Name" };
      s.ResponseExamples[200] = new UpdateTimsMachineResponse(new TimsMachineRecord("Class Type", 0, "Domain Name", "External IP Address", "External Port Number", "Internal IP Address", "Internal Port Number", "1000", "Narration", true, "Serial Number", "Tims  Name", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateTimsMachineRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MachineID))
    {
      AddError(request => request.MachineID, "The machine id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<TimsMachineDTO, TimsMachine>(CreateEndPointUser.GetEndPointUser(User), request.MachineID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the tims machine to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<TimsMachineDTO, TimsMachine>(CreateEndPointUser.GetEndPointUser(User), request.MachineID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateTimsMachineResponse(new TimsMachineRecord(obj.ClassType, obj.CurrentStatus, obj.DomainName, obj.ExternalIPAddress, obj.ExternalPortNumber, obj.InternalIPAddress, obj.InternalPortNumber, obj.Id, obj.Narration, obj.ReadyForUse, obj.SerialNumber, obj.TimsName, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
