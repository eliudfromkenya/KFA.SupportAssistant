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

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// Update an existing actual budget variance.
/// </summary>
/// <remarks>
/// Update an existing actual budget variance by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateActualBudgetVarianceRequest, UpdateActualBudgetVarianceResponse>
{
  private const string EndPointId = "ENP-107";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateActualBudgetVarianceRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Actual Budget Variance End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Actual Budget Variance";
      s.Description = "This endpoint is used to update  actual budget variance, making a full replacement of actual budget variance with a specifed valuse. A valid actual budget variance is required.";
      s.ExampleRequest = new UpdateActualBudgetVarianceRequest { ActualBudgetID = "1000", ActualValue = 0, BatchKey = string.Empty, BudgetValue = 0, Comment = "Comment", Description = "Description", Field1 = "Field 1", Field2 = "Field 2", Field3 = "Field 3", LedgerCode = string.Empty, LedgerCostCentreCode = string.Empty, Narration = "Narration" };
      s.ResponseExamples[200] = new UpdateActualBudgetVarianceResponse(new ActualBudgetVarianceRecord("1000", 0, string.Empty, 0, "Comment", "Description", "Field 1", "Field 2", "Field 3", string.Empty, string.Empty, "Narration", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateActualBudgetVarianceRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ActualBudgetID))
    {
      AddError(request => request.ActualBudgetID, "The actual budget id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ActualBudgetVarianceDTO, ActualBudgetVariance>(CreateEndPointUser.GetEndPointUser(User), request.ActualBudgetID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the actual budget variance to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ActualBudgetVarianceDTO, ActualBudgetVariance>(CreateEndPointUser.GetEndPointUser(User), request.ActualBudgetID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateActualBudgetVarianceResponse(new ActualBudgetVarianceRecord(obj.Id, obj.ActualValue, obj.BatchKey, obj.BudgetValue, obj.Comment, obj.Description, obj.Field1, obj.Field2, obj.Field3, obj.LedgerCode, obj.LedgerCostCentreCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
