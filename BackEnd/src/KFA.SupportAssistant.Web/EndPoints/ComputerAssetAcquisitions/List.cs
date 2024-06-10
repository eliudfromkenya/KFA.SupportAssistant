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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

/// <summary>
/// List all computer asset acquisitions by specified conditions
/// </summary>
/// <remarks>
/// List all computer asset acquisitions - returns a ComputerAssetAcquisitionListResponse containing the computer asset acquisitions.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetAcquisitionListResponse>
{
  private const string EndPointId = "ENP-145";
  public const string Route = "/computer_asset_acquisitions";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Asset Acquisitions List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer asset acquisitions as specified";
      s.Description = "Returns all computer asset acquisitions as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetAcquisitionListResponse { ComputerAssetAcquisitions = [new ComputerAssetAcquisitionRecord("1000", "Aquisition Type", "", 0, DateTime.Now, "Document No", "Narration", "Quotation Number", "Received By", 0, 0, "Vendor Code", DateTime.Now, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetAcquisitionDTO, ComputerAssetAcquisition>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetAcquisitionDTO>>.Success(ans.Select(v => (ComputerAssetAcquisitionDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetAcquisitionListResponse
      {
        ComputerAssetAcquisitions = result.Value.Select(obj => new ComputerAssetAcquisitionRecord(obj.Id, obj.AquisitionType, obj.AssetDetailID, obj.AssetValue, obj.DateOfAcquisition, obj.DocumentNo, obj.Narration, obj.QuotationNumber, obj.ReceivedBy, obj.Value, obj.VATAmount, obj.VendorCode, obj.WarantyEndDate, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
