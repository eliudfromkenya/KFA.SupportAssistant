using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// Delete a user right.
/// </summary>
/// <remarks>
/// Delete a user right by providing a valid user right id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteUserRightRequest>
{
  private const string EndPointId = "ENP-252";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteUserRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete User Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete user right";
      s.Description = "This endpoint is used to delete user right with specified (provided) user right id(s)";
      s.ExampleRequest = new DeleteUserRightRequest { UserRightId = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteUserRightRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.UserRightId))
    {
      AddError(request => request.UserRightId, "The user right id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<UserRight>(CreateEndPointUser.GetEndPointUser(User), request.UserRightId ?? "");
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
