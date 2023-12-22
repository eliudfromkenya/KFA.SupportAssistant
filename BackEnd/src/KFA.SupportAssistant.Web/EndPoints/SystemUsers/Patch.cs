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

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchSystemUserRequest, SystemUserRecord>
{
  private const string EndPointId = "ENP-216";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchSystemUserRequest.Route));
    //RequestBinder(new PatchBinder<SystemUserDTO, SystemUser, PatchSystemUserRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update System User End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a system user";
      s.Description = "Used to update part of an existing system user. A valid existing system user is required.";
      s.ResponseExamples[200] = new SystemUserRecord("Contact", "Email Address", DateTime.Now, true, DateTime.Now, "Name Of The User", "Narration", string.Empty, "1000", "Username", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchSystemUserRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.UserId))
    {
      AddError(request => request.UserId, "The system user of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    SystemUserDTO patchFunc(SystemUserDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<SystemUserDTO, SystemUser>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<SystemUserDTO, SystemUser>(CreateEndPointUser.GetEndPointUser(User), request.UserId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the system user to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new SystemUserRecord(obj.Contact, obj.EmailAddress, obj.ExpirationDate, obj.IsActive, obj.MaturityDate, obj.NameOfTheUser, obj.Narration, obj.RoleId, obj.Id, obj.Username, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
