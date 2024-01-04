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

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

/// <summary>
/// List all expenses budget details by specified conditions
/// </summary>
/// <remarks>
/// List all expenses budget details - returns a ExpensesBudgetDetailListResponse containing the expenses budget details.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ExpensesBudgetDetailListResponse>
{
  private const string EndPointId = "ENP-1D5";
  public const string Route = "/expenses_budget_details";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Expenses Budget Details List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of expenses budget details as specified";
      s.Description = "Returns all expenses budget details as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ExpensesBudgetDetailListResponse { ExpensesBudgetDetails = [new ExpensesBudgetDetailRecord("Basis Of Calculation", "Batch Key", "1000", "Ledger Account Code", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "Narration", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ExpensesBudgetDetailDTO, ExpensesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<ExpensesBudgetDetailDTO>>.Success(ans.Select(v => (ExpensesBudgetDetailDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ExpensesBudgetDetailListResponse
      {
        ExpensesBudgetDetails = result.Value.Select(obj => new ExpensesBudgetDetailRecord(obj.BasisOfCalculation, obj.BatchKey, obj.Id, obj.LedgerAccountCode, obj.Month01, obj.Month02, obj.Month03, obj.Month04, obj.Month05, obj.Month06, obj.Month07, obj.Month08, obj.Month09, obj.Month10, obj.Month11, obj.Month12, obj.Narration, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
