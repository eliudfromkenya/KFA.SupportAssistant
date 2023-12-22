using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

/// <summary>
/// Create a new UserLogin
/// </summary>
/// <remarks>
/// Creates a new user login given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateUserLoginRequest, CreateUserLoginResponse>
{
  private const string EndPointId = "ENP-241";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateUserLoginRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add User Login End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new user login";
      s.Description = "This endpoint is used to create a new  user login. Here details of user login to be created is provided";
      s.ExampleRequest = new CreateUserLoginRequest { DeviceId = string.Empty, FromDate = DateTime.Now, LoginId = "1000", Narration = "Narration", UptoDate = DateTime.Now, UserId = string.Empty };
      s.ResponseExamples[200] = new CreateUserLoginResponse(string.Empty, DateTime.Now, "1000", "Narration", DateTime.Now, string.Empty, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateUserLoginRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<UserLoginDTO>();
    requestDTO.Id = request.LoginId;

    var result = await mediator.Send(new CreateModelCommand<UserLoginDTO, UserLogin>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is UserLoginDTO obj)
      {
        Response = new CreateUserLoginResponse(obj.DeviceId, obj.FromDate, obj.Id, obj.Narration, obj.UptoDate, obj.UserId, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
