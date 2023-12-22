using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

/// <summary>
/// Get a communication message by message id.
/// </summary>
/// <remarks>
/// Takes message id and returns a matching communication message record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetCommunicationMessageByIdRequest, CommunicationMessageRecord>
{
  private const string EndPointId = "ENP-134";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetCommunicationMessageByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Communication Message End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets communication message by specified message id";
      s.Description = "This endpoint is used to retrieve communication message with the provided message id";
      s.ExampleRequest = new GetCommunicationMessageByIdRequest { MessageID = "message id to retrieve" };
      s.ResponseExamples[200] = new CommunicationMessageRecord(new byte[] { }, "Details", "From", "Message", "1000",  Core.DataLayer.Types.CommunicationMessageType.SMS, "Narration", Core.DataLayer.Types.CommunicationMessageStatus.Send, "Title", "To", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetCommunicationMessageByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.MessageID))
    {
      AddError(request => request.MessageID, "The message id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<CommunicationMessageDTO, CommunicationMessage>(CreateEndPointUser.GetEndPointUser(User), request.MessageID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new CommunicationMessageRecord(obj.Attachments, obj.Details, obj.From, obj.Message, obj.Id, obj.MessageType, obj.Narration, obj.Status, obj.Title, obj.To, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
