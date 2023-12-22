using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// Get a count sheet batch by batch key.
/// </summary>
/// <remarks>
/// Takes batch key and returns a matching count sheet batch record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetCountSheetBatchByIdRequest, CountSheetBatchRecord>
{
  private const string EndPointId = "ENP-164";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetCountSheetBatchByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Count Sheet Batch End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets count sheet batch by specified batch key";
      s.Description = "This endpoint is used to retrieve count sheet batch with the provided batch key";
      s.ExampleRequest = new GetCountSheetBatchByIdRequest { BatchKey = "batch key to retrieve" };
      s.ResponseExamples[200] = new CountSheetBatchRecord("1000", "Batch Number", "Class Of Card", 0, 0, "Cost Centre Code", DateTime.Now, "Month", "Narration", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetCountSheetBatchByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The batch key of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<CountSheetBatchDTO, CountSheetBatch>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "");
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
      Response = new CountSheetBatchRecord(obj.Id, obj.BatchNumber, obj.ClassOfCard, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.Month, obj.Narration, obj.NoOfRecords, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
