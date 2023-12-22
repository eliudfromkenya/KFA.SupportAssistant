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

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchCommunicationMessageRequest, CommunicationMessageRecord>
{
  private const string EndPointId = "ENP-136";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchCommunicationMessageRequest.Route));
    //RequestBinder(new PatchBinder<CommunicationMessageDTO, CommunicationMessage, PatchCommunicationMessageRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Communication Message End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a communication message";
      s.Description = "Used to update part of an existing communication message. A valid existing communication message is required.";
      s.ResponseExamples[200] = new CommunicationMessageRecord(new byte[] { }, "Details", "From", "Message", "1000",  Core.DataLayer.Types.CommunicationMessageType.WhatsApp, "Narration", Core.DataLayer.Types.CommunicationMessageStatus.Delivered, "Title", "To", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchCommunicationMessageRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MessageID))
    {
      AddError(request => request.MessageID, "The communication message of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    CommunicationMessageDTO patchFunc(CommunicationMessageDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<CommunicationMessageDTO, CommunicationMessage>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<CommunicationMessageDTO, CommunicationMessage>(CreateEndPointUser.GetEndPointUser(User), request.MessageID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the communication message to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new CommunicationMessageRecord(obj.Attachments, obj.Details, obj.From, obj.Message, obj.Id, obj.MessageType, obj.Narration, obj.Status, obj.Title, obj.To, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
