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

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchActualBudgetVarianceRequest, ActualBudgetVarianceRecord>
{
  private const string EndPointId = "ENP-106";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchActualBudgetVarianceRequest.Route));
    //RequestBinder(new PatchBinder<ActualBudgetVarianceDTO, ActualBudgetVariance, PatchActualBudgetVarianceRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Actual Budget Variance End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a actual budget variance";
      s.Description = "Used to update part of an existing actual budget variance. A valid existing actual budget variance is required.";
      s.ResponseExamples[200] = new ActualBudgetVarianceRecord("1000", 0, string.Empty, 0, "Comment", "Description", "Field 1", "Field 2", "Field 3", string.Empty, string.Empty, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchActualBudgetVarianceRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ActualBudgetID))
    {
      AddError(request => request.ActualBudgetID, "The actual budget variance of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ActualBudgetVarianceDTO patchFunc(ActualBudgetVarianceDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ActualBudgetVarianceDTO, ActualBudgetVariance>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ActualBudgetVarianceDTO, ActualBudgetVariance>(CreateEndPointUser.GetEndPointUser(User), request.ActualBudgetID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the actual budget variance to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ActualBudgetVarianceRecord(obj.Id, obj.ActualValue, obj.BatchKey, obj.BudgetValue, obj.Comment, obj.Description, obj.Field1, obj.Field2, obj.Field3, obj.LedgerCode, obj.LedgerCostCentreCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
