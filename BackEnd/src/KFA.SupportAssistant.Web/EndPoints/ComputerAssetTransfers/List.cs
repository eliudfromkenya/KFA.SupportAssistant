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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// List all computer asset transfers by specified conditions
/// </summary>
/// <remarks>
/// List all computer asset transfers - returns a ComputerAssetTransferListResponse containing the computer asset transfers.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetTransferListResponse>
{
  private const string EndPointId = "ENP-195";
  public const string Route = "/computer_asset_transfers";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Asset Transfers List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer asset transfers as specified";
      s.Description = "Returns all computer asset transfers as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetTransferListResponse { ComputerAssetTransfers = [new ComputerAssetTransferRecord("", "Cost Centre Code", "Narration", "Payroll Number", "Responsible User", "Status", DateTime.Now, "1000", "Transfer Reasons", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetTransferDTO, ComputerAssetTransfer>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetTransferDTO>>.Success(ans.Select(v => (ComputerAssetTransferDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetTransferListResponse
      {
        ComputerAssetTransfers = result.Value.Select(obj => new ComputerAssetTransferRecord(obj.AssetDetailID, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.ResponsibleUser, obj.Status, obj.TransferDate, obj.Id, obj.TransferReasons, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
