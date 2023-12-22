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

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchDefaultAccessRightRequest, DefaultAccessRightRecord>
{
  private const string EndPointId = "ENP-186";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchDefaultAccessRightRequest.Route));
    //RequestBinder(new PatchBinder<DefaultAccessRightDTO, DefaultAccessRight, PatchDefaultAccessRightRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Default Access Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a default access right";
      s.Description = "Used to update part of an existing default access right. A valid existing default access right is required.";
      s.ResponseExamples[200] = new DefaultAccessRightRecord("Name", "Narration", "1000", "Rights", "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchDefaultAccessRightRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RightID))
    {
      AddError(request => request.RightID, "The default access right of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    DefaultAccessRightDTO patchFunc(DefaultAccessRightDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<DefaultAccessRightDTO, DefaultAccessRight>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<DefaultAccessRightDTO, DefaultAccessRight>(CreateEndPointUser.GetEndPointUser(User), request.RightID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the default access right to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new DefaultAccessRightRecord(obj.Name, obj.Narration, obj.Id, obj.Rights, obj.Type, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
