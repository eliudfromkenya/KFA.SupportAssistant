using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// Create a new TimsMachine
/// </summary>
/// <remarks>
/// Creates a new tims machine given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateTimsMachineRequest, CreateTimsMachineResponse>
{
  private const string EndPointId = "ENP-221";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateTimsMachineRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Tims Machine End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new tims machine";
      s.Description = "This endpoint is used to create a new  tims machine. Here details of tims machine to be created is provided";
      s.ExampleRequest = new CreateTimsMachineRequest { ClassType = "Class Type", CurrentStatus = 0, DomainName = "Domain Name", ExternalIPAddress = "External IP Address", ExternalPortNumber = "External Port Number", InternalIPAddress = "Internal IP Address", InternalPortNumber = "Internal Port Number", MachineID = "1000", Narration = "Narration", ReadyForUse = true, SerialNumber = "Serial Number", TimsName = "Tims  Name" };
      s.ResponseExamples[200] = new CreateTimsMachineResponse("Class Type", 0, "Domain Name", "External IP Address", "External Port Number", "Internal IP Address", "Internal Port Number", "1000", "Narration", true, "Serial Number", "Tims  Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateTimsMachineRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<TimsMachineDTO>();
    requestDTO.Id = request.MachineID;

    var result = await mediator.Send(new CreateModelCommand<TimsMachineDTO, TimsMachine>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is TimsMachineDTO obj)
      {
        Response = new CreateTimsMachineResponse(obj.ClassType, obj.CurrentStatus, obj.DomainName, obj.ExternalIPAddress, obj.ExternalPortNumber, obj.InternalIPAddress, obj.InternalPortNumber, obj.Id, obj.Narration, obj.ReadyForUse, obj.SerialNumber, obj.TimsName, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
