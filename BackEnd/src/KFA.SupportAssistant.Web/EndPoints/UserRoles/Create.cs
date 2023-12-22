using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserRoles;

/// <summary>
/// Create a new UserRole
/// </summary>
/// <remarks>
/// Creates a new user role given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateUserRoleRequest, CreateUserRoleResponse>
{
  private const string EndPointId = "ENP-261";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateUserRoleRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add User Role End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new user role";
      s.Description = "This endpoint is used to create a new  user role. Here details of user role to be created is provided";
      s.ExampleRequest = new CreateUserRoleRequest { ExpirationDate = DateTime.Now, MaturityDate = DateTime.Now, Narration = "Narration", RoleId = "1000", RoleName = "Role Name" };
      s.ResponseExamples[200] = new CreateUserRoleResponse(DateTime.Now, DateTime.Now, "Narration", "1000", "Role Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateUserRoleRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<UserRoleDTO>();
    requestDTO.Id = request.RoleId;

    var result = await mediator.Send(new CreateModelCommand<UserRoleDTO, UserRole>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is UserRoleDTO obj)
      {
        Response = new CreateUserRoleResponse(obj.ExpirationDate, obj.MaturityDate, obj.Narration, obj.Id, obj.RoleName, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
