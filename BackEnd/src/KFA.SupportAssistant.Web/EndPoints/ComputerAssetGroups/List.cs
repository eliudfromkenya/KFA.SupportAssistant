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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// List all computer asset groups by specified conditions
/// </summary>
/// <remarks>
/// List all computer asset groups - returns a ComputerAssetGroupListResponse containing the computer asset groups.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetGroupListResponse>
{
  private const string EndPointId = "ENP-175";
  public const string Route = "/computer_asset_groups";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Asset Groups List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer asset groups as specified";
      s.Description = "Returns all computer asset groups as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetGroupListResponse { ComputerAssetGroups = [new ComputerAssetGroupRecord(true, "Description", "1000", "Group Name", "Last Assigned Value", "", "Prefix", "Suffix", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetGroupDTO, ComputerAssetGroup>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetGroupDTO>>.Success(ans.Select(v => (ComputerAssetGroupDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetGroupListResponse
      {
        ComputerAssetGroups = result.Value.Select(obj => new ComputerAssetGroupRecord(obj.CanBeAssigned, obj.Description, obj.Id, obj.GroupName, obj.LastAssignedValue, obj.ParentGroupID, obj.Prefix, obj.Suffix, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
