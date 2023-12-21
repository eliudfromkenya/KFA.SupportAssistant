using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.EndPoints.CostCentres;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.UseCases.Models.Update;

public class PatchCostCentre(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchCostCentreRequest, CostCentreRecord>
{
  private const string EndPointId = "ENP-016";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchCostCentreRequest.Route));
    //RequestBinder(new PatchBinder<CostCentreDTO, CostCentre, PatchCostCentreRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Cost Centre End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Update partially a cost centre";
      s.Description = "Updates part of an existing CostCentre. A valid existing is required.";
      s.ResponseExamples[200] = new CostCentreRecord("Id", "Name", "Narration", "Region", "Supplier Code", true, DateTime.UtcNow, DateTime.UtcNow);
    });
  }

  public override async Task HandleAsync(PatchCostCentreRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CostCentreCode))
    {
      AddError(request => request.CostCentreCode, "Id of item to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    CostCentreDTO patchFunc(CostCentreDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<CostCentreDTO, CostCentre>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.CostCentreCode ?? string.Empty, patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the cost centre to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var value = result.Value;
    if (result.IsSuccess)
    {
      Response = new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix, value?.IsActive, value?.DateInserted___, value?.DateUpdated___);
    }
  }
}
