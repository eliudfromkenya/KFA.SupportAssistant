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

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchUserAuditTrailRequest, UserAuditTrailRecord>
{
  private const string EndPointId = "ENP-236";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchUserAuditTrailRequest.Route));
    //RequestBinder(new PatchBinder<UserAuditTrailDTO, UserAuditTrail, PatchUserAuditTrailRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update User Audit Trail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a user audit trail";
      s.Description = "Used to update part of an existing user audit trail. A valid existing user audit trail is required.";
      s.ResponseExamples[200] = new UserAuditTrailRecord(DateTime.Now, 0, "1000", "Category", string.Empty, "Data", "Description", string.Empty, "Narration", "Old Values", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchUserAuditTrailRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AuditId))
    {
      AddError(request => request.AuditId, "The user audit trail of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    UserAuditTrailDTO patchFunc(UserAuditTrailDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<UserAuditTrailDTO, UserAuditTrail>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<UserAuditTrailDTO, UserAuditTrail>(CreateEndPointUser.GetEndPointUser(User), request.AuditId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the user audit trail to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UserAuditTrailRecord(obj.ActivityDate, obj.ActivityEnumNumber, obj.Id, obj.Category, obj.CommandId, obj.Data, obj.Description, obj.LoginId, obj.Narration, obj.OldValues, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
