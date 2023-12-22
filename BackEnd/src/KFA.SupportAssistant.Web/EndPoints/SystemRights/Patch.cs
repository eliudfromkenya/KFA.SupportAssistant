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

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchSystemRightRequest, SystemRightRecord>
{
  private const string EndPointId = "ENP-206";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchSystemRightRequest.Route));
    //RequestBinder(new PatchBinder<SystemRightDTO, SystemRight, PatchSystemRightRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update System Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a system right";
      s.Description = "Used to update part of an existing system right. A valid existing system right is required.";
      s.ResponseExamples[200] = new SystemRightRecord(true, "Narration", "1000", "Right Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchSystemRightRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RightId))
    {
      AddError(request => request.RightId, "The system right of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    SystemRightDTO patchFunc(SystemRightDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<SystemRightDTO, SystemRight>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<SystemRightDTO, SystemRight>(CreateEndPointUser.GetEndPointUser(User), request.RightId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the system right to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new SystemRightRecord(obj.IsCompulsory, obj.Narration, obj.Id, obj.RightName, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
