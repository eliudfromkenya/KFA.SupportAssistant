using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// Create a new SystemUser
/// </summary>
/// <remarks>
/// Creates a new system user given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateSystemUserRequest, CreateSystemUserResponse>
{
  private const string EndPointId = "ENP-211";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateSystemUserRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add System User End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new system user";
      s.Description = "This endpoint is used to create a new  system user. Here details of system user to be created is provided";
      s.ExampleRequest = new CreateSystemUserRequest { Contact = "Contact", EmailAddress = "Email Address", ExpirationDate = DateTime.Now, IsActive = true, MaturityDate = DateTime.Now, NameOfTheUser = "Name Of The User", Narration = "Narration", PasswordHash = new byte[] { }, PasswordSalt = new byte[] { }, RoleId = string.Empty, UserId = "1000", Username = "Username" };
      s.ResponseExamples[200] = new CreateSystemUserResponse("Contact", "Email Address", DateTime.Now, true, DateTime.Now, "Name Of The User", "Narration", string.Empty, "1000", "Username", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateSystemUserRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<SystemUserDTO>();
    requestDTO.Id = request.UserId;

    var result = await mediator.Send(new CreateModelCommand<SystemUserDTO, SystemUser>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is SystemUserDTO obj)
      {
        Response = new CreateSystemUserResponse(obj.Contact, obj.EmailAddress, obj.ExpirationDate, obj.IsActive, obj.MaturityDate, obj.NameOfTheUser, obj.Narration, obj.RoleId, obj.Id, obj.Username, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
