using Ardalis.Result;
using FastEndpoints;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.EndPoints.CostCentres;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.UseCases.Models.Update;

public class PatchCostCentre(IMediator mediator) : Endpoint<PatchRequest, CostCentreRecord>
{
  public override void Configure()
  {
    Patch(PatchRequest.Route);
    AllowAnonymous();
    //Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    //PreProcessor<PatchRequestChecker<PatchRequest,string>,string>();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Update partially a cost centre";
      s.Description = "Updates part of an existing CostCentre. A valid existing is required.";
      //s.ExampleRequest = new PatchRequest { Id = "Id to update", Content = @"{{Description: ""New Cost center Name""}}" };
      s.ResponseExamples[200] = new CostCentreRecord("Id", "Name", "Narration", "Region", "Supplier Code", DateTime.UtcNow, DateTime.UtcNow);
    });
  }

  public override async Task HandleAsync(PatchRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Id))
    {
      AddError(request => request.Id ?? "Id", "Id of item to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    //var state = ProcessorState<MyStateBag>();

    CostCentreDTO patchFunc(CostCentreDTO tt) => PatchUpdater.Patch<CostCentreDTO, CostCentre>(() => request.PatchDocument, request.Content, tt);
    var result = await mediator.Send(new PatchModelCommand<CostCentreDTO, CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.Id ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the cost centre to update");

    //var state = ProcessorState<MyStateBag>();

    if (result.Errors.Any())
      result.Errors.ToList().ForEach(n => AddError(n));
    await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    ThrowIfAnyErrors();

    var value = result.Value;
    if (result.IsSuccess)
    {
      Response = new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix, value?.DateInserted___, value?.DateUpdated___);
    }
  }
}
