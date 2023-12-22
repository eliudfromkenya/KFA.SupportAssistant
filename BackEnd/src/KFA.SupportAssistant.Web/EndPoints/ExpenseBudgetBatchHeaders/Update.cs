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

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// Update an existing expense budget batch header.
/// </summary>
/// <remarks>
/// Update an existing expense budget batch header by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateExpenseBudgetBatchHeaderRequest, UpdateExpenseBudgetBatchHeaderResponse>
{
  private const string EndPointId = "ENP-1C7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateExpenseBudgetBatchHeaderRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Expense Budget Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Expense Budget Batch Header";
      s.Description = "This endpoint is used to update  expense budget batch header, making a full replacement of expense budget batch header with a specifed valuse. A valid expense budget batch header is required.";
      s.ExampleRequest = new UpdateExpenseBudgetBatchHeaderRequest { ApprovedBy = "Approved By", BatchKey = "1000", BatchNumber = "Batch Number", ComputerNumberOfRecords = 0, ComputerTotalAmount = 0, CostCentreCode = "Cost Centre Code", Date = DateTime.Now, MonthFrom = "Month From", MonthTo = "Month To", Narration = "Narration", NumberOfRecords = 0, PreparedBy = "Prepared By", TotalAmount = 0 };
      s.ResponseExamples[200] = new UpdateExpenseBudgetBatchHeaderResponse(new ExpenseBudgetBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateExpenseBudgetBatchHeaderRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The batch key of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the expense budget batch header to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateExpenseBudgetBatchHeaderResponse(new ExpenseBudgetBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
