using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Endpoints.CostCentreEndpoints;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CostCentres;

/// <summary>
/// Delete a CostCentre.
/// </summary>
/// <remarks>
/// Delete a CostCentre by providing a valid integer id.
/// </remarks>
public class Delete(IMediator mediator) : Endpoint<DeleteCostCentreRequest>
{
  public override void Configure()
  {
    Delete(DeleteCostCentreRequest.Route);
    Permissions(UserRoleConstants.RIGHT_SYSTEM_ROUTINES, UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_SUPERVISOR, UserRoleConstants.ROLE_MANAGER);
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = "Delete a Cost Centre";
      s.Description = "Used to delete cost centre with specified id(s)";
      s.ExampleRequest = new DeleteCostCentreRequest { Id = "AAA-01" };
      s.ResponseExamples = new Dictionary<int, object> { { 200, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteCostCentreRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Id))
    {
      AddError(request => request.Id ?? "Id", "Item to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<CostCentre>(CreateEndPointUser.GetEndPointUser(User), request.Id ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
      result.Errors.ToList().ForEach(n => AddError(n));
    await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    };
  }
}
