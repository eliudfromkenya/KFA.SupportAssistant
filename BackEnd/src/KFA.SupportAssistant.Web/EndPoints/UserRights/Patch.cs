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

namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchUserRightRequest, UserRightRecord>
{
  private const string EndPointId = "ENP-256";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchUserRightRequest.Route));
    //RequestBinder(new PatchBinder<UserRightDTO, UserRight, PatchUserRightRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update User Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a user right";
      s.Description = "Used to update part of an existing user right. A valid existing user right is required.";
      s.ResponseExamples[200] = new UserRightRecord("Description", "Narration", "Object Name", string.Empty, string.Empty, string.Empty, "1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchUserRightRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.UserRightId))
    {
      AddError(request => request.UserRightId, "The user right of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    UserRightDTO patchFunc(UserRightDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<UserRightDTO, UserRight>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<UserRightDTO, UserRight>(CreateEndPointUser.GetEndPointUser(User), request.UserRightId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the user right to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UserRightRecord(obj.Description, obj.Narration, obj.ObjectName, obj.RightId, obj.RoleId, obj.UserId, obj.Id, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
