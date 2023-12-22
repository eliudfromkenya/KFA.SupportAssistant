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

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchUserLoginRequest, UserLoginRecord>
{
  private const string EndPointId = "ENP-246";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchUserLoginRequest.Route));
    //RequestBinder(new PatchBinder<UserLoginDTO, UserLogin, PatchUserLoginRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update User Login End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a user login";
      s.Description = "Used to update part of an existing user login. A valid existing user login is required.";
      s.ResponseExamples[200] = new UserLoginRecord(string.Empty, DateTime.Now, "1000", "Narration", DateTime.Now, string.Empty, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchUserLoginRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LoginId))
    {
      AddError(request => request.LoginId, "The user login of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    UserLoginDTO patchFunc(UserLoginDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<UserLoginDTO, UserLogin>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<UserLoginDTO, UserLogin>(CreateEndPointUser.GetEndPointUser(User), request.LoginId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the user login to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UserLoginRecord(obj.DeviceId, obj.FromDate, obj.Id, obj.Narration, obj.UptoDate, obj.UserId, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
