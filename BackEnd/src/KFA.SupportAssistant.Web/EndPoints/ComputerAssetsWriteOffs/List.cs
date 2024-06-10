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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// List all computer assets write offs by specified conditions
/// </summary>
/// <remarks>
/// List all computer assets write offs - returns a ComputerAssetsWriteOffListResponse containing the computer assets write offs.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetsWriteOffListResponse>
{
  private const string EndPointId = "ENP-1G5";
  public const string Route = "/computer_assets_write_offs";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Assets Write Offs List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer assets write offs as specified";
      s.Description = "Returns all computer assets write offs as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetsWriteOffListResponse { ComputerAssetsWriteOffs = [new ComputerAssetsWriteOffRecord("", "Description", "Narration", "Reason For Write-Off", "1000", DateTime.Now, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetsWriteOffDTO>>.Success(ans.Select(v => (ComputerAssetsWriteOffDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetsWriteOffListResponse
      {
        ComputerAssetsWriteOffs = result.Value.Select(obj => new ComputerAssetsWriteOffRecord(obj.AssetDetailID, obj.Description, obj.Narration, obj.ReasonForWriteOff, obj.Id, obj.WriteOffDate, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
