using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

/// <summary>
/// List all actual budget variances batch headers by specified conditions
/// </summary>
/// <remarks>
/// List all actual budget variances batch headers - returns a ActualBudgetVariancesBatchHeaderListResponse containing the actual budget variances batch headers.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ActualBudgetVariancesBatchHeaderListResponse>
{
  private const string EndPointId = "ENP-115";
  public const string Route = "/actual_budget_variances_batch_headers";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Actual Budget Variances Batch Headers List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of actual budget variances batch headers as specified";
      s.Description = "Returns all actual budget variances batch headers as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ActualBudgetVariancesBatchHeaderListResponse { ActualBudgetVariancesBatchHeaders = [new ActualBudgetVariancesBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, 0, 0, "Cost Centre Code", "Month", "Narration", 0, "Prepared By", 0, 0, 0, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ActualBudgetVariancesBatchHeaderDTO, ActualBudgetVariancesBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ActualBudgetVariancesBatchHeaderListResponse
      {
        ActualBudgetVariancesBatchHeaders = result.Value.Select(obj => new ActualBudgetVariancesBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.CashSalesAmount, obj.ComputerNumberOfRecords, obj.ComputerTotalActualAmount, obj.ComputerTotalBudgetAmount, obj.CostCentreCode, obj.Month, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.PurchasesesAmount, obj.TotalActualAmount, obj.TotalBudgetAmount, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
