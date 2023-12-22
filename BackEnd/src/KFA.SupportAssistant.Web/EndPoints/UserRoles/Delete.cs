using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// Delete a user role.
/// </summary>
/// <remarks>
/// Delete a user role by providing a valid role id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteUserRoleRequest>
{
  private const string EndPointId = "ENP-262";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteUserRoleRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete User Role End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete user role";
      s.Description = "This endpoint is used to delete user role with specified (provided) role id(s)";
      s.ExampleRequest = new DeleteUserRoleRequest { RoleId = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteUserRoleRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RoleId))
    {
      AddError(request => request.RoleId, "The role id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<UserRole>(CreateEndPointUser.GetEndPointUser(User), request.RoleId ?? "");
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
