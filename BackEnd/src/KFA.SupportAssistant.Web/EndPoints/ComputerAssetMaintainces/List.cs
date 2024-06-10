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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

/// <summary>
/// List all computer asset maintainces by specified conditions
/// </summary>
/// <remarks>
/// List all computer asset maintainces - returns a ComputerAssetMaintainceListResponse containing the computer asset maintainces.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetMaintainceListResponse>
{
  private const string EndPointId = "ENP-185";
  public const string Route = "/computer_asset_maintainces";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Asset Maintainces List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer asset maintainces as specified";
      s.Description = "Returns all computer asset maintainces as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetMaintainceListResponse { ComputerAssetMaintainces = [new ComputerAssetMaintainceRecord("", "", "Asset State", "Description", "Diagnosis", "Done By", "1000", "Narration", true, "What Was Done", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetMaintainceDTO, ComputerAssetMaintaince>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetMaintainceDTO>>.Success(ans.Select(v => (ComputerAssetMaintainceDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetMaintainceListResponse
      {
        ComputerAssetMaintainces = result.Value.Select(obj => new ComputerAssetMaintainceRecord(obj.AssetDispatchID, obj.AssetRecieveID, obj.AssetState, obj.Description, obj.Diagnosis, obj.DoneBy, obj.Id, obj.Narration, obj.TreatAsExpense, obj.WhatWasDone, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
