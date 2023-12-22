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

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchExpenseBudgetBatchHeaderRequest, ExpenseBudgetBatchHeaderRecord>
{
  private const string EndPointId = "ENP-1C6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchExpenseBudgetBatchHeaderRequest.Route));
    //RequestBinder(new PatchBinder<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader, PatchExpenseBudgetBatchHeaderRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Expense Budget Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a expense budget batch header";
      s.Description = "Used to update part of an existing expense budget batch header. A valid existing expense budget batch header is required.";
      s.ResponseExamples[200] = new ExpenseBudgetBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchExpenseBudgetBatchHeaderRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The expense budget batch header of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ExpenseBudgetBatchHeaderDTO patchFunc(ExpenseBudgetBatchHeaderDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the expense budget batch header to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ExpenseBudgetBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
