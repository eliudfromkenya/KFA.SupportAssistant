using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PurchasesBudgetDetails;

/// <summary>
/// Delete a purchases budget detail.
/// </summary>
/// <remarks>
/// Delete a purchases budget detail by providing a valid purchases budget detail id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeletePurchasesBudgetDetailRequest>
{
  private const string EndPointId = "ENP-1Q2";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeletePurchasesBudgetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete Purchases Budget Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete purchases budget detail";
      s.Description = "This endpoint is used to delete purchases budget detail with specified (provided) purchases budget detail id(s)";
      s.ExampleRequest = new DeletePurchasesBudgetDetailRequest { PurchasesBudgetDetailId = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeletePurchasesBudgetDetailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PurchasesBudgetDetailId))
    {
      AddError(request => request.PurchasesBudgetDetailId, "The purchases budget detail id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<PurchasesBudgetDetail>(CreateEndPointUser.GetEndPointUser(User), request.PurchasesBudgetDetailId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

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
