
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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

/// <summary>
/// List all computer assets maintaince accessories by specified conditions
/// </summary>
/// <remarks>
/// List all computer assets maintaince accessories - returns a ComputerAssetsMaintainceAccessoryListResponse containing the computer assets maintaince accessories.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ComputerAssetsMaintainceAccessoryListResponse>
{
  private const string EndPointId = "ENP-1B5";
  public const string Route = "/computer_assets_maintaince_accessories";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Computer Assets Maintaince Accessories List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of computer assets maintaince accessories as specified";
      s.Description = "Returns all computer assets maintaince accessories as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ComputerAssetsMaintainceAccessoryListResponse { ComputerAssetsMaintainceAccessories = [new ComputerAssetsMaintainceAccessoryRecord("", "1000", 0, "", DateTime.Now, "Description", "Invoice Number", "", "Narration", "Quotation Number", 0, "Vendor Code", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ComputerAssetsMaintainceAccessoryDTO, ComputerAssetsMaintainceAccessory>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ComputerAssetsMaintainceAccessoryDTO>>.Success(ans.Select(v => (ComputerAssetsMaintainceAccessoryDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ComputerAssetsMaintainceAccessoryListResponse
      {
        ComputerAssetsMaintainceAccessories = result.Value.Select(obj => new ComputerAssetsMaintainceAccessoryRecord(obj.AccessoryGroupID, obj.Id, obj.Amount, obj.AssetDetailID, obj.DateOfAcquisition, obj.Description, obj.InvoiceNumber, obj.MaintainceID, obj.Narration, obj.QuotationNumber, obj.VATAmount, obj.VendorCode, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
