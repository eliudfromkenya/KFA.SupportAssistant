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

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// Update an existing system user.
/// </summary>
/// <remarks>
/// Update an existing system user by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateSystemUserRequest, UpdateSystemUserResponse>
{
  private const string EndPointId = "ENP-217";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateSystemUserRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update System User End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full System User";
      s.Description = "This endpoint is used to update  system user, making a full replacement of system user with a specifed valuse. A valid system user is required.";
      s.ExampleRequest = new UpdateSystemUserRequest { Contact = "Contact", EmailAddress = "Email Address", ExpirationDate = DateTime.Now, IsActive = true, MaturityDate = DateTime.Now, NameOfTheUser = "Name Of The User", Narration = "Narration", PasswordHash = new byte[] { }, PasswordSalt = new byte[] { }, RoleId = string.Empty, UserId = "1000", Username = "Username" };
      s.ResponseExamples[200] = new UpdateSystemUserResponse(new SystemUserRecord("Contact", "Email Address", DateTime.Now, true, DateTime.Now, "Name Of The User", "Narration", string.Empty, "1000", "Username", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateSystemUserRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.UserId))
    {
      AddError(request => request.UserId, "The user id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<SystemUserDTO, SystemUser>(CreateEndPointUser.GetEndPointUser(User), request.UserId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the system user to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<SystemUserDTO, SystemUser>(CreateEndPointUser.GetEndPointUser(User), request.UserId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateSystemUserResponse(new SystemUserRecord(obj.Contact, obj.EmailAddress, obj.ExpirationDate, obj.IsActive, obj.MaturityDate, obj.NameOfTheUser, obj.Narration, obj.RoleId, obj.Id, obj.Username, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
