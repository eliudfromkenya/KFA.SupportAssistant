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

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// Update an existing user role.
/// </summary>
/// <remarks>
/// Update an existing user role by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateUserRoleRequest, UpdateUserRoleResponse>
{
  private const string EndPointId = "ENP-267";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateUserRoleRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update User Role End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full User Role";
      s.Description = "This endpoint is used to update  user role, making a full replacement of user role with a specifed valuse. A valid user role is required.";
      s.ExampleRequest = new UpdateUserRoleRequest { ExpirationDate = DateTime.Now, MaturityDate = DateTime.Now, Narration = "Narration", RoleId = "1000", RoleName = "Role Name" };
      s.ResponseExamples[200] = new UpdateUserRoleResponse(new UserRoleRecord(DateTime.Now, DateTime.Now, "Narration", "1000", "Role Name", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateUserRoleRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RoleId))
    {
      AddError(request => request.RoleId, "The role id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<UserRoleDTO, UserRole>(CreateEndPointUser.GetEndPointUser(User), request.RoleId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the user role to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<UserRoleDTO, UserRole>(CreateEndPointUser.GetEndPointUser(User), request.RoleId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateUserRoleResponse(new UserRoleRecord(obj.ExpirationDate, obj.MaturityDate, obj.Narration, obj.Id, obj.RoleName, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
