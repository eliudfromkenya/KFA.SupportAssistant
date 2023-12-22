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

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetBatchHeaders;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchPurchasesBudgetBatchHeaderRequest, PurchasesBudgetBatchHeaderRecord>
{
  private const string EndPointId = "ENP-1P6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchPurchasesBudgetBatchHeaderRequest.Route));
    //RequestBinder(new PatchBinder<PurchasesBudgetBatchHeaderDTO, PurchasesBudgetBatchHeader, PatchPurchasesBudgetBatchHeaderRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Purchases Budget Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a purchases budget batch header";
      s.Description = "Used to update part of an existing purchases budget batch header. A valid existing purchases budget batch header is required.";
      s.ResponseExamples[200] = new PurchasesBudgetBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchPurchasesBudgetBatchHeaderRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The purchases budget batch header of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    PurchasesBudgetBatchHeaderDTO patchFunc(PurchasesBudgetBatchHeaderDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<PurchasesBudgetBatchHeaderDTO, PurchasesBudgetBatchHeader>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<PurchasesBudgetBatchHeaderDTO, PurchasesBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the purchases budget batch header to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new PurchasesBudgetBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.TotalQuantity, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
