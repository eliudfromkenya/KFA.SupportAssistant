using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// List all tims machines by specified conditions
/// </summary>
/// <remarks>
/// List all tims machines - returns a TimsMachineListResponse containing the tims machines.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, TimsMachineListResponse>
{
  private const string EndPointId = "ENP-225";
  public const string Route = "/tims_machines";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Tims Machines List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of tims machines as specified";
      s.Description = "Returns all tims machines as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new TimsMachineListResponse { TimsMachines = [new TimsMachineRecord("Class Type", 0, "Domain Name", "External IP Address", "External Port Number", "Internal IP Address", "Internal Port Number", "1000", "Narration", true, "Serial Number", "Tims  Name", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<TimsMachineDTO, TimsMachine>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new TimsMachineListResponse
      {
        TimsMachines = result.Value.Select(obj => new TimsMachineRecord(obj.ClassType, obj.CurrentStatus, obj.DomainName, obj.ExternalIPAddress, obj.ExternalPortNumber, obj.InternalIPAddress, obj.InternalPortNumber, obj.Id, obj.Narration, obj.ReadyForUse, obj.SerialNumber, obj.TimsName, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
