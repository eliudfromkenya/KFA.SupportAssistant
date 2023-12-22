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

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchUserRoleRequest, UserRoleRecord>
{
  private const string EndPointId = "ENP-266";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchUserRoleRequest.Route));
    //RequestBinder(new PatchBinder<UserRoleDTO, UserRole, PatchUserRoleRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update User Role End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a user role";
      s.Description = "Used to update part of an existing user role. A valid existing user role is required.";
      s.ResponseExamples[200] = new UserRoleRecord(DateTime.Now, DateTime.Now, "Narration", "1000", "Role Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchUserRoleRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RoleId))
    {
      AddError(request => request.RoleId, "The user role of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    UserRoleDTO patchFunc(UserRoleDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<UserRoleDTO, UserRole>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<UserRoleDTO, UserRole>(CreateEndPointUser.GetEndPointUser(User), request.RoleId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the user role to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UserRoleRecord(obj.ExpirationDate, obj.MaturityDate, obj.Narration, obj.Id, obj.RoleName, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
