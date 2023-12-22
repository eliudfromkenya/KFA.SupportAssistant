using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// Update an existing price change request.
/// </summary>
/// <remarks>
/// Update an existing price change request by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdatePriceChangeRequestRequest, UpdatePriceChangeRequestResponse>
{
  private const string EndPointId = "ENP-1N7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdatePriceChangeRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Price Change Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Price Change Request";
      s.Description = "This endpoint is used to update  price change request, making a full replacement of price change request with a specifed valuse. A valid price change request is required.";
      s.ExampleRequest = new UpdatePriceChangeRequestRequest { AttandedBy = "Attanded By", BatchNumber = "Batch Number", CostCentreCode = "Cost Centre Code", CostPrice = "Cost Price", ItemCode = "Item Code", Narration = "Narration", RequestID = "1000", RequestingUser = "Requesting User", SellingPrice = "Selling Price", Status = "Status", TimeAttended = "Time Attended", TimeOfRequest = "Time of Request" };
      s.ResponseExamples[200] = new UpdatePriceChangeRequestResponse(new PriceChangeRequestRecord("Attanded By", "Batch Number", "Cost Centre Code", "Cost Price", "Item Code", "Narration", "1000", "Requesting User", "Selling Price", "Status", DateTime.Now,DateTime.Now, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdatePriceChangeRequestRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RequestID))
    {
      AddError(request => request.RequestID, "The request id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<PriceChangeRequestDTO, PriceChangeRequest>(CreateEndPointUser.GetEndPointUser(User), request.RequestID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the price change request to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<PriceChangeRequestDTO, PriceChangeRequest>(CreateEndPointUser.GetEndPointUser(User), request.RequestID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdatePriceChangeRequestResponse(new PriceChangeRequestRecord(obj.AttandedBy, obj.BatchNumber, obj.CostCentreCode, obj.CostPrice, obj.ItemCode, obj.Narration, obj.Id, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
