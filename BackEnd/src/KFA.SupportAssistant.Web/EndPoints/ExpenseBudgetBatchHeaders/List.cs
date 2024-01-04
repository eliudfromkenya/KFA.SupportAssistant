using Ardalis.Result;
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

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// List all expense budget batch headers by specified conditions
/// </summary>
/// <remarks>
/// List all expense budget batch headers - returns a ExpenseBudgetBatchHeaderListResponse containing the expense budget batch headers.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ExpenseBudgetBatchHeaderListResponse>
{
  private const string EndPointId = "ENP-1C5";
  public const string Route = "/expense_budget_batch_headers";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Expense Budget Batch Headers List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of expense budget batch headers as specified";
      s.Description = "Returns all expense budget batch headers as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ExpenseBudgetBatchHeaderListResponse { ExpenseBudgetBatchHeaders = [new ExpenseBudgetBatchHeaderRecord("Approved By", "1000", "Batch Number", 0, 0, "Cost Centre Code", DateTime.Now, "Month From", "Month To", "Narration", 0, "Prepared By", 0, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ExpenseBudgetBatchHeaderDTO, ExpenseBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ExpenseBudgetBatchHeaderDTO>>.Success(ans.Select(v => (ExpenseBudgetBatchHeaderDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ExpenseBudgetBatchHeaderListResponse
      {
        ExpenseBudgetBatchHeaders = result.Value.Select(obj => new ExpenseBudgetBatchHeaderRecord(obj.ApprovedBy, obj.Id, obj.BatchNumber, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.MonthFrom, obj.MonthTo, obj.Narration, obj.NumberOfRecords, obj.PreparedBy, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
