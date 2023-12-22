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

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchActualBudgetVariancesBatchHeaderRequest, ActualBudgetVariancesBatchHeaderRecord>
{
  private const string EndPointId = "ENP-116";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchActualBudgetVariancesBatchHeaderRequest.Route));
    //RequestBinder(new PatchBinder<ActualBudgetVariancesBatchHeaderDTO, ActualBudgetVariancesBatchHeader, PatchActualBudgetVariancesBatchHeaderRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Actual Budget Variances Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a actual budget variances batch header";
      s.Description = "Used to update part of an existing actual budget variances batch header. A valid existing actual budget variances batch header is required.";
      s.ResponseExamples[200] = new ActualBudgetVariancesBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, 0, 0, "Cost Centre Code", "Month", "Narration", 0, "Prepared By", 0, 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchActualBudgetVariancesBatchHeaderRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The actual budget variances batch header of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ActualBudgetVariancesBatchHeaderDTO patchFunc(ActualBudgetVariancesBatchHeaderDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ActualBudgetVariancesBatchHeaderDTO, ActualBudgetVariancesBatchHeader>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ActualBudgetVariancesBatchHeaderDTO, ActualBudgetVariancesBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the actual budget variances batch header to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ActualBudgetVariancesBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.CashSalesAmount, obj.ComputerNumberOfRecords, obj.ComputerTotalActualAmount, obj.ComputerTotalBudgetAmount, obj.CostCentreCode, obj.Month, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.PurchasesesAmount, obj.TotalActualAmount, obj.TotalBudgetAmount, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
