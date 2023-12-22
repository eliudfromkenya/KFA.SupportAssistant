using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// Get a tims machine by machine id.
/// </summary>
/// <remarks>
/// Takes machine id and returns a matching tims machine record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetTimsMachineByIdRequest, TimsMachineRecord>
{
  private const string EndPointId = "ENP-224";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetTimsMachineByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Tims Machine End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets tims machine by specified machine id";
      s.Description = "This endpoint is used to retrieve tims machine with the provided machine id";
      s.ExampleRequest = new GetTimsMachineByIdRequest { MachineID = "machine id to retrieve" };
      s.ResponseExamples[200] = new TimsMachineRecord("Class Type", 0, "Domain Name", "External IP Address", "External Port Number", "Internal IP Address", "Internal Port Number", "1000", "Narration", true, "Serial Number", "Tims  Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetTimsMachineByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MachineID))
    {
      AddError(request => request.MachineID, "The machine id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<TimsMachineDTO, TimsMachine>(CreateEndPointUser.GetEndPointUser(User), request.MachineID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new TimsMachineRecord(obj.ClassType, obj.CurrentStatus, obj.DomainName, obj.ExternalIPAddress, obj.ExternalPortNumber, obj.InternalIPAddress, obj.InternalPortNumber, obj.Id, obj.Narration, obj.ReadyForUse, obj.SerialNumber, obj.TimsName, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
