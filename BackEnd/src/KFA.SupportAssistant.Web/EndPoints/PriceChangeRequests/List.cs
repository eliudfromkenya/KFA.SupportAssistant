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

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// List all price change requests by specified conditions
/// </summary>
/// <remarks>
/// List all price change requests - returns a PriceChangeRequestListResponse containing the price change requests.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, PriceChangeRequestListResponse>
{
  private const string EndPointId = "ENP-1N5";
  public const string Route = "/price_change_requests";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Price Change Requests List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of price change requests as specified";
      s.Description = "Returns all price change requests as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new PriceChangeRequestListResponse { PriceChangeRequests = [new PriceChangeRequestRecord("Attanded By", "Batch Number", "Cost Centre Code", "Cost Price", "Item Code", "Narration", "1000", "Requesting User", "Selling Price", "Status", DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<PriceChangeRequestDTO, PriceChangeRequest>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new PriceChangeRequestListResponse
      {
        PriceChangeRequests = result.Value.Select(obj => new PriceChangeRequestRecord(obj.AttandedBy, obj.BatchNumber, obj.CostCentreCode, obj.CostPrice, obj.ItemCode, obj.Narration, obj.Id, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
