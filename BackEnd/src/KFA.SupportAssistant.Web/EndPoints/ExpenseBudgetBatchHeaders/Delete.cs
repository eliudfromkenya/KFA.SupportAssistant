using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ExpenseBudgetBatchHeaders;

/// <summary>
/// Delete a expense budget batch header.
/// </summary>
/// <remarks>
/// Delete a expense budget batch header by providing a valid batch key.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteExpenseBudgetBatchHeaderRequest>
{
  private const string EndPointId = "ENP-1C2";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteExpenseBudgetBatchHeaderRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete Expense Budget Batch Header End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete expense budget batch header";
      s.Description = "This endpoint is used to delete expense budget batch header with specified (provided) batch key(s)";
      s.ExampleRequest = new DeleteExpenseBudgetBatchHeaderRequest { BatchKey = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteExpenseBudgetBatchHeaderRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.BatchKey))
    {
      AddError(request => request.BatchKey, "The batch key of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<ExpenseBudgetBatchHeader>(CreateEndPointUser.GetEndPointUser(User), request.BatchKey ?? "");
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
