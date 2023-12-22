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

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchVerificationRightRequest, VerificationRightRecord>
{
  private const string EndPointId = "ENP-286";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchVerificationRightRequest.Route));
    //RequestBinder(new PatchBinder<VerificationRightDTO, VerificationRight, PatchVerificationRightRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Verification Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a verification right";
      s.Description = "Used to update part of an existing verification right. A valid existing verification right is required.";
      s.ResponseExamples[200] = new VerificationRightRecord(string.Empty, string.Empty, string.Empty, "1000", 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchVerificationRightRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationRightId))
    {
      AddError(request => request.VerificationRightId, "The verification right of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    VerificationRightDTO patchFunc(VerificationRightDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<VerificationRightDTO, VerificationRight>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<VerificationRightDTO, VerificationRight>(CreateEndPointUser.GetEndPointUser(User), request.VerificationRightId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the verification right to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new VerificationRightRecord(obj.DeviceId, obj.UserId, obj.UserRoleId, obj.Id, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
