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

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// Update an existing count sheet batch.
/// </summary>
/// <remarks>
/// Update an existing count sheet batch by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateCountSheetBatchRequest, UpdateCountSheetBatchResponse>
{
  private const string EndPointId = "ENP-167";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateCountSheetBatchRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Count Sheet Batch End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Count Sheet Batch";
      s.Description = "This endpoint is used to update  count sheet batch, making a full replacement of count sheet batch with a specifed valuse. A valid count sheet batch is required.";
      s.ExampleRequest = new UpdateCountSheetBatchRequest { BatchKey = "1000", BatchNumber = "Batch Number", ClassOfCard = "Class Of Card", ComputerNumberOfRecords = 0, ComputerTotalAmount = 0, CostCentreCode = "Cost Centre Code", Date = DateTime.Now, Month = "Month", Narration = "Narration", NoOfRecords = 0, TotalAmount = 0 };
      s.ResponseExamples[200] = new UpdateCountSheetBatchResponse(new CountSheetBatchRecord("1000", "Batch Number", "Class Of Card", 0, 0, "Cost Centre Code", DateTime.Now, "Month", "Narration", 0, 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateCountSheetBatchRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The batch key of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<CountSheetBatchDTO, CountSheetBatch>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the count sheet batch to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<CountSheetBatchDTO, CountSheetBatch>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateCountSheetBatchResponse(new CountSheetBatchRecord(obj.Id, obj.BatchNumber, obj.ClassOfCard, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.Month, obj.Narration, obj.NoOfRecords, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
