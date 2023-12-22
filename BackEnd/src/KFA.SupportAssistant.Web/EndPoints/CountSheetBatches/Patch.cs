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

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchCountSheetBatchRequest, CountSheetBatchRecord>
{
  private const string EndPointId = "ENP-166";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchCountSheetBatchRequest.Route));
    //RequestBinder(new PatchBinder<CountSheetBatchDTO, CountSheetBatch, PatchCountSheetBatchRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Count Sheet Batch End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a count sheet batch";
      s.Description = "Used to update part of an existing count sheet batch. A valid existing count sheet batch is required.";
      s.ResponseExamples[200] = new CountSheetBatchRecord("1000", "Batch Number", "Class Of Card", 0, 0, "Cost Centre Code", DateTime.Now, "Month", "Narration", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchCountSheetBatchRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The count sheet batch of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    CountSheetBatchDTO patchFunc(CountSheetBatchDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<CountSheetBatchDTO, CountSheetBatch>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<CountSheetBatchDTO, CountSheetBatch>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the count sheet batch to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new CountSheetBatchRecord(obj.Id, obj.BatchNumber, obj.ClassOfCard, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.Month, obj.Narration, obj.NoOfRecords, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
