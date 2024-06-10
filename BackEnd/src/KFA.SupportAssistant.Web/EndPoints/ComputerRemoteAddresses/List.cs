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
using Ardalis.Result;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerRemoteAddresses;

/// <summary>
/// List all computer remote addresses by specified conditions
/// </summary>
/// <remarks>
/// List all computer remote addresses - returns a ComputerRemoteAddressListResponse containing the computer remote addresses.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerRemoteAddressListResponse>
{
  private const string EndPointId = "ENP-1H5";
  public const string Route = "/computer_remote_addresses";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Remote Addresses List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer remote addresses as specified";
      s.Description = "Returns all computer remote addresses as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerRemoteAddressListResponse { ComputerRemoteAddresses = [new ComputerRemoteAddressRecord("1000", "AnyDesk Number", "Anydesk Password", "", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Team Viewer Address", "Type", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerRemoteAddressDTO, ComputerRemoteAddress>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerRemoteAddressDTO>>.Success(ans.Select(v => (ComputerRemoteAddressDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerRemoteAddressListResponse
      {
        ComputerRemoteAddresses = result.Value.Select(obj => new ComputerRemoteAddressRecord(obj.Id, obj.AnyDeskNumber, obj.AnydeskPassword, obj.AssetDetailID, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.TeamViewerAddress, obj.Type, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
