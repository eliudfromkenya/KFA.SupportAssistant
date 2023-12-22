using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

/// <summary>
/// Create a new CommunicationMessage
/// </summary>
/// <remarks>
/// Creates a new communication message given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateCommunicationMessageRequest, CreateCommunicationMessageResponse>
{
  private const string EndPointId = "ENP-131";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateCommunicationMessageRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Communication Message End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new communication message";
      s.Description = "This endpoint is used to create a new  communication message. Here details of communication message to be created is provided";
      s.ExampleRequest = new CreateCommunicationMessageRequest { Attachments = new byte[] { }, Details = "Details", From = "From", Message = "Message", MessageID = "1000", MessageType = "Message Type", Narration = "Narration", Status = "Status", Title = "Title", To = "To" };
      s.ResponseExamples[200] = new CreateCommunicationMessageResponse(new byte[] { }, "Details", "From", "Message", "1000", Core.DataLayer.Types.CommunicationMessageType.Email, "Narration", Core.DataLayer.Types.CommunicationMessageStatus.Received, "Title", "To", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateCommunicationMessageRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<CommunicationMessageDTO>();
    requestDTO.Id = request.MessageID;

    var result = await mediator.Send(new CreateModelCommand<CommunicationMessageDTO, CommunicationMessage>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is CommunicationMessageDTO obj)
      {
        Response = new CreateCommunicationMessageResponse(obj.Attachments, obj.Details, obj.From, obj.Message, obj.Id, obj.MessageType, obj.Narration, obj.Status, obj.Title, obj.To, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
