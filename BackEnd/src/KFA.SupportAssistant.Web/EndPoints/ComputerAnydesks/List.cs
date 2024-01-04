using Ardalis.Result;
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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

/// <summary>
/// List all computer anydesks by specified conditions
/// </summary>
/// <remarks>
/// List all computer anydesks - returns a ComputerAnydeskListResponse containing the computer anydesks.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAnydeskListResponse>
{
  private const string EndPointId = "ENP-145";
  public const string Route = "/computer_anydesks";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Anydesks List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer anydesks as specified";
      s.Description = "Returns all computer anydesks as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAnydeskListResponse { ComputerAnydesks = [new ComputerAnydeskRecord("1000", "AnyDesk Number", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Password", Core.DataLayer.Types.AnyDeskComputerType.BackOffice, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAnydeskDTO, ComputerAnydesk>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAnydeskDTO>>.Success(ans.Select(v => (ComputerAnydeskDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAnydeskListResponse
      {
        ComputerAnydesks = result.Value.Select(obj => new ComputerAnydeskRecord(obj.Id, obj.AnyDeskNumber, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.Password, obj.Type, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
