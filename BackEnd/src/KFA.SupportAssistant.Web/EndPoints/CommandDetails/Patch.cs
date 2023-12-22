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

namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchCommandDetailRequest, CommandDetailRecord>
{
  private const string EndPointId = "ENP-126";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchCommandDetailRequest.Route));
    //RequestBinder(new PatchBinder<CommandDetailDTO, CommandDetail, PatchCommandDetailRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Command Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a command detail";
      s.Description = "Used to update part of an existing command detail. A valid existing command detail is required.";
      s.ResponseExamples[200] = new CommandDetailRecord("Action", "Active State", "Category", "1000", "Command Name", "Command Text", 0, "Image Path", true, true, "Narration", "Shortcut Key", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchCommandDetailRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.CommandId))
    {
      AddError(request => request.CommandId, "The command detail of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    CommandDetailDTO patchFunc(CommandDetailDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<CommandDetailDTO, CommandDetail>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<CommandDetailDTO, CommandDetail>(CreateEndPointUser.GetEndPointUser(User), request.CommandId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the command detail to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new CommandDetailRecord(obj.Action, obj.ActiveState, obj.Category, obj.Id, obj.CommandName, obj.CommandText, obj.ImageId, obj.ImagePath, obj.IsEnabled, obj.IsPublished, obj.Narration, obj.ShortcutKey, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
