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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// List all computer assets posts to dynamics by specified conditions
/// </summary>
/// <remarks>
/// List all computer assets posts to dynamics - returns a ComputerAssetsPostsToDynamicListResponse containing the computer assets posts to dynamics.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetsPostsToDynamicListResponse>
{
  private const string EndPointId = "ENP-1E5";
  public const string Route = "/computer_assets_posts_to_dynamics";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Assets Posts To Dynamics List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer assets posts to dynamics as specified";
      s.Description = "Returns all computer assets posts to dynamics as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetsPostsToDynamicListResponse { ComputerAssetsPostsToDynamics = [new ComputerAssetsPostsToDynamicRecord("", DateTime.Now, "Description", "1000", true, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetsPostsToDynamicDTO>>.Success(ans.Select(v => (ComputerAssetsPostsToDynamicDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetsPostsToDynamicListResponse
      {
        ComputerAssetsPostsToDynamics = result.Value.Select(obj => new ComputerAssetsPostsToDynamicRecord(obj.AssetDetailID, obj.DateGenerated, obj.Description, obj.Id, obj.Posted, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
