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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// List all computer assets maintaince receivings by specified conditions
/// </summary>
/// <remarks>
/// List all computer assets maintaince receivings - returns a ComputerAssetsMaintainceReceivingListResponse containing the computer assets maintaince receivings.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetsMaintainceReceivingListResponse>
{
  private const string EndPointId = "ENP-1D5";
  public const string Route = "/computer_assets_maintaince_receivings";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Assets Maintaince Receivings List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer assets maintaince receivings as specified";
      s.Description = "Returns all computer assets maintaince receivings as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetsMaintainceReceivingListResponse { ComputerAssetsMaintainceReceivings = [new ComputerAssetsMaintainceReceivingRecord("", "Brought By", DateTime.Now, "Description", "From Department Code", "Narration", "Reason To Send", "1000", "Recieved By", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetsMaintainceReceivingDTO>>.Success(ans.Select(v => (ComputerAssetsMaintainceReceivingDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetsMaintainceReceivingListResponse
      {
        ComputerAssetsMaintainceReceivings = result.Value.Select(obj => new ComputerAssetsMaintainceReceivingRecord(obj.AssetDetailID, obj.BroughtBy, obj.DateRecieved, obj.Description, obj.FromDepartmentCode, obj.Narration, obj.ReasonToSend, obj.Id, obj.RecievedBy, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
