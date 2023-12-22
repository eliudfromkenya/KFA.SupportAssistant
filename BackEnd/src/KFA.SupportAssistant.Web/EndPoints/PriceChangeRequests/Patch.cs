using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchPriceChangeRequestRequest, PriceChangeRequestRecord>
{
  private const string EndPointId = "ENP-1N6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchPriceChangeRequestRequest.Route));
    //RequestBinder(new PatchBinder<PriceChangeRequestDTO, PriceChangeRequest, PatchPriceChangeRequestRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Price Change Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a price change request";
      s.Description = "Used to update part of an existing price change request. A valid existing price change request is required.";
      s.ResponseExamples[200] = new PriceChangeRequestRecord("Attanded By", "Batch Number", "Cost Centre Code", "Cost Price", "Item Code", "Narration", "1000", "Requesting User", "Selling Price", "Status", DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchPriceChangeRequestRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RequestID))
    {
      AddError(request => request.RequestID, "The price change request of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    PriceChangeRequestDTO patchFunc(PriceChangeRequestDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<PriceChangeRequestDTO, PriceChangeRequest>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<PriceChangeRequestDTO, PriceChangeRequest>(CreateEndPointUser.GetEndPointUser(User), request.RequestID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the price change request to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new PriceChangeRequestRecord(obj.AttandedBy, obj.BatchNumber, obj.CostCentreCode, obj.CostPrice, obj.ItemCode, obj.Narration, obj.Id, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
