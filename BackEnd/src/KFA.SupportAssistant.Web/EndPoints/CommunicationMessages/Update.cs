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

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

/// <summary>
/// Update an existing communication message.
/// </summary>
/// <remarks>
/// Update an existing communication message by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateCommunicationMessageRequest, UpdateCommunicationMessageResponse>
{
  private const string EndPointId = "ENP-137";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateCommunicationMessageRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Communication Message End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Communication Message";
      s.Description = "This endpoint is used to update  communication message, making a full replacement of communication message with a specifed valuse. A valid communication message is required.";
      s.ExampleRequest = new UpdateCommunicationMessageRequest { Attachments = new byte[] { }, Details = "Details", From = "From", Message = "Message", MessageID = "1000", MessageType = Core.DataLayer.Types.CommunicationMessageType.WhatsApp, Narration = "Narration", Status =  Core.DataLayer.Types.CommunicationMessageStatus.Delivered, Title = "Title", To = "To" };
      s.ResponseExamples[200] = new UpdateCommunicationMessageResponse(new CommunicationMessageRecord(new byte[] { }, "Details", "From", "Message", "1000", Core.DataLayer.Types.CommunicationMessageType.WhatsApp, "Narration",  Core.DataLayer.Types.CommunicationMessageStatus.Undelivered, "Title", "To", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateCommunicationMessageRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MessageID))
    {
      AddError(request => request.MessageID, "The message id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<CommunicationMessageDTO, CommunicationMessage>(CreateEndPointUser.GetEndPointUser(User), request.MessageID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the communication message to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<CommunicationMessageDTO, CommunicationMessage>(CreateEndPointUser.GetEndPointUser(User), request.MessageID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateCommunicationMessageResponse(new CommunicationMessageRecord(obj.Attachments, obj.Details, obj.From, obj.Message, obj.Id, obj.MessageType, obj.Narration, obj.Status, obj.Title, obj.To, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
