using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// Get a price change request by request id.
/// </summary>
/// <remarks>
/// Takes request id and returns a matching price change request record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetPriceChangeRequestByIdRequest, PriceChangeRequestRecord>
{
  private const string EndPointId = "ENP-1N4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetPriceChangeRequestByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Price Change Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets price change request by specified request id";
      s.Description = "This endpoint is used to retrieve price change request with the provided request id";
      s.ExampleRequest = new GetPriceChangeRequestByIdRequest { RequestID = "request id to retrieve" };
      s.ResponseExamples[200] = new PriceChangeRequestRecord("Attanded By", "Batch Number", "Cost Centre Code", "Cost Price", "Item Code", "Narration", "1000", "Requesting User", "Selling Price", "Status", DateTime.Now,DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetPriceChangeRequestByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RequestID))
    {
      AddError(request => request.RequestID, "The request id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<PriceChangeRequestDTO, PriceChangeRequest>(CreateEndPointUser.GetEndPointUser(User), request.RequestID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new PriceChangeRequestRecord(obj.AttandedBy, obj.BatchNumber, obj.CostCentreCode, obj.CostPrice, obj.ItemCode, obj.Narration, obj.Id, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
