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

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// List all actual budget variances by specified conditions
/// </summary>
/// <remarks>
/// List all actual budget variances - returns a ActualBudgetVarianceListResponse containing the actual budget variances.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ActualBudgetVarianceListResponse>
{
  private const string EndPointId = "ENP-105";
  public const string Route = "/actual_budget_variances";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Actual Budget Variances List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of actual budget variances as specified";
      s.Description = "Returns all actual budget variances as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ActualBudgetVarianceListResponse { ActualBudgetVariances = [new ActualBudgetVarianceRecord("1000", 0,string.Empty, 0, "Comment", "Description", "Field 1", "Field 2", "Field 3", string.Empty, string.Empty, "Narration", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ActualBudgetVarianceDTO, ActualBudgetVariance>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ActualBudgetVarianceListResponse
      {
        ActualBudgetVariances = result.Value.Select(obj => new ActualBudgetVarianceRecord(obj.Id, obj.ActualValue, obj.BatchKey, obj.BudgetValue, obj.Comment, obj.Description, obj.Field1, obj.Field2, obj.Field3, obj.LedgerCode, obj.LedgerCostCentreCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
