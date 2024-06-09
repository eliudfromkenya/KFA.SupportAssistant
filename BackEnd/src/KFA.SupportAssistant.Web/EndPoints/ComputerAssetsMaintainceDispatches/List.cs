
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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// List all computer assets maintaince dispatches by specified conditions
/// </summary>
/// <remarks>
/// List all computer assets maintaince dispatches - returns a ComputerAssetsMaintainceDispatchListResponse containing the computer assets maintaince dispatches.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetsMaintainceDispatchListResponse>
{
  private const string EndPointId = "ENP-1C5";
  public const string Route = "/computer_assets_maintaince_dispatches";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Assets Maintaince Dispatches List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer assets maintaince dispatches as specified";
      s.Description = "Returns all computer assets maintaince dispatches as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetsMaintainceDispatchListResponse { ComputerAssetsMaintainceDispatches = [new ComputerAssetsMaintainceDispatchRecord("", "Collected By", DateTime.Now, "Description", "1000", "Narration", "Reason To Send", "Sent By", "To Department Code", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetsMaintainceDispatchDTO>>.Success(ans.Select(v => (ComputerAssetsMaintainceDispatchDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetsMaintainceDispatchListResponse
      {
        ComputerAssetsMaintainceDispatches = result.Value.Select(obj => new ComputerAssetsMaintainceDispatchRecord(obj.AssetDetailID, obj.CollectedBy, obj.DateSend, obj.Description, obj.Id, obj.Narration, obj.ReasonToSend, obj.SentBy, obj.ToDepartmentCode, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
