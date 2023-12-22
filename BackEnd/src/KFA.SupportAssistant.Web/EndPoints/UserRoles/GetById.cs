using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// Get a user role by role id.
/// </summary>
/// <remarks>
/// Takes role id and returns a matching user role record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetUserRoleByIdRequest, UserRoleRecord>
{
  private const string EndPointId = "ENP-264";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetUserRoleByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get User Role End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets user role by specified role id";
      s.Description = "This endpoint is used to retrieve user role with the provided role id";
      s.ExampleRequest = new GetUserRoleByIdRequest { RoleId = "role id to retrieve" };
      s.ResponseExamples[200] = new UserRoleRecord(DateTime.Now, DateTime.Now, "Narration", "1000", "Role Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetUserRoleByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RoleId))
    {
      AddError(request => request.RoleId, "The role id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<UserRoleDTO, UserRole>(CreateEndPointUser.GetEndPointUser(User), request.RoleId ?? "");
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
      Response = new UserRoleRecord(obj.ExpirationDate, obj.MaturityDate, obj.Narration, obj.Id, obj.RoleName, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
