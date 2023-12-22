using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

/// <summary>
/// Create a new UserRight
/// </summary>
/// <remarks>
/// Creates a new user right given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateUserRightRequest, CreateUserRightResponse>
{
  private const string EndPointId = "ENP-251";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateUserRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add User Right End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new user right";
      s.Description = "This endpoint is used to create a new  user right. Here details of user right to be created is provided";
      s.ExampleRequest = new CreateUserRightRequest { Description = "Description", Narration = "Narration", ObjectName = "Object Name", RightId = string.Empty, RoleId = string.Empty, UserId = string.Empty, UserRightId = "1000" };
      s.ResponseExamples[200] = new CreateUserRightResponse("Description", "Narration", "Object Name", string.Empty, string.Empty, string.Empty,"1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateUserRightRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<UserRightDTO>();
    requestDTO.Id = request.UserRightId;

    var result = await mediator.Send(new CreateModelCommand<UserRightDTO, UserRight>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is UserRightDTO obj)
      {
        Response = new CreateUserRightResponse(obj.Description, obj.Narration, obj.ObjectName, obj.RightId, obj.RoleId, obj.UserId, obj.Id, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
