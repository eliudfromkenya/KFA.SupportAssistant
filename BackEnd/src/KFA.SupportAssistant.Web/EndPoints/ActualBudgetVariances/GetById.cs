using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// Get a actual budget variance by actual budget id.
/// </summary>
/// <remarks>
/// Takes actual budget id and returns a matching actual budget variance record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetActualBudgetVarianceByIdRequest, ActualBudgetVarianceRecord>
{
  private const string EndPointId = "ENP-104";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetActualBudgetVarianceByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Actual Budget Variance End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets actual budget variance by specified actual budget id";
      s.Description = "This endpoint is used to retrieve actual budget variance with the provided actual budget id";
      s.ExampleRequest = new GetActualBudgetVarianceByIdRequest { ActualBudgetID = "actual budget id to retrieve" };
      s.ResponseExamples[200] = new ActualBudgetVarianceRecord("1000", 0, string.Empty, 0, "Comment", "Description", "Field 1", "Field 2", "Field 3", string.Empty, string.Empty, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetActualBudgetVarianceByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ActualBudgetID))
    {
      AddError(request => request.ActualBudgetID, "The actual budget id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ActualBudgetVarianceDTO, ActualBudgetVariance>(CreateEndPointUser.GetEndPointUser(User), request.ActualBudgetID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ActualBudgetVarianceRecord(obj.Id, obj.ActualValue, obj.BatchKey, obj.BudgetValue, obj.Comment, obj.Description, obj.Field1, obj.Field2, obj.Field3, obj.LedgerCode, obj.LedgerCostCentreCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
