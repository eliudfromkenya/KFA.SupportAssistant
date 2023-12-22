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

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

/// <summary>
/// Update an existing actual budget variances batch header.
/// </summary>
/// <remarks>
/// Update an existing actual budget variances batch header by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateActualBudgetVariancesBatchHeaderRequest, UpdateActualBudgetVariancesBatchHeaderResponse>
{
  private const string EndPointId = "ENP-117";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateActualBudgetVariancesBatchHeaderRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Actual Budget Variances Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Actual Budget Variances Batch Header";
      s.Description = "This endpoint is used to update  actual budget variances batch header, making a full replacement of actual budget variances batch header with a specifed valuse. A valid actual budget variances batch header is required.";
      s.ExampleRequest = new UpdateActualBudgetVariancesBatchHeaderRequest { ApprovedBy = "Approved By", BatchKey = "1000", BatchNumber = "Batch Number", CashSalesAmount = 0, ComputerNumberOfRecords = 0, ComputerTotalActualAmount = 0, ComputerTotalBudgetAmount = 0, CostCentreCode = "Cost Centre Code", Month = "Month", Narration = "Narration", NumberOfRecords = 0, PreparedBy = "Prepared By", PurchasesesAmount = 0, TotalActualAmount = 0, TotalBudgetAmount = 0 };
      s.ResponseExamples[200] = new UpdateActualBudgetVariancesBatchHeaderResponse(new ActualBudgetVariancesBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, 0, 0, "Cost Centre Code", "Month", "Narration", 0, "Prepared By", 0, 0, 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateActualBudgetVariancesBatchHeaderRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The batch key of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ActualBudgetVariancesBatchHeaderDTO, ActualBudgetVariancesBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the actual budget variances batch header to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ActualBudgetVariancesBatchHeaderDTO, ActualBudgetVariancesBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateActualBudgetVariancesBatchHeaderResponse(new ActualBudgetVariancesBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.CashSalesAmount, obj.ComputerNumberOfRecords, obj.ComputerTotalActualAmount, obj.ComputerTotalBudgetAmount, obj.CostCentreCode, obj.Month, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.PurchasesesAmount, obj.TotalActualAmount, obj.TotalBudgetAmount, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
