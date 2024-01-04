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

namespace KFA.SupportAssistant.Web.EndPoints.StockItemCodesRequests;

/// <summary>
/// List all stock item codes requests by specified conditions
/// </summary>
/// <remarks>
/// List all stock item codes requests - returns a StockItemCodesRequestListResponse containing the stock item codes requests.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, StockItemCodesRequestListResponse>
{
  private const string EndPointId = "ENP-1X5";
  public const string Route = "/stock_item_codes_requests";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Stock Item Codes Requests List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of stock item codes requests as specified";
      s.Description = "Returns all stock item codes requests as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new StockItemCodesRequestListResponse { StockItemCodesRequests = [new StockItemCodesRequestRecord("Attanded By", "Cost Centre Code", 0, "Description", "Distributor", "Item Code", "1000", "Narration", "Requesting User", 0, "Status", "Supplier", DateTime.Now, DateTime.Now, "Unit Of Measure", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<StockItemCodesRequestDTO, StockItemCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<StockItemCodesRequestDTO>>.Success(ans.Select(v => (StockItemCodesRequestDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new StockItemCodesRequestListResponse
      {
        StockItemCodesRequests = result.Value.Select(obj => new StockItemCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.CostPrice, obj.Description, obj.Distributor, obj.ItemCode, obj.Id, obj.Narration, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.Supplier, obj.TimeAttended, obj.TimeOfRequest, obj.UnitOfMeasure, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
