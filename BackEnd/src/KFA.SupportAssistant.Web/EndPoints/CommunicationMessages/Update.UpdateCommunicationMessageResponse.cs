namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

public class UpdateCommunicationMessageResponse
{
  public UpdateCommunicationMessageResponse(CommunicationMessageRecord communicationMessage)
  {
    CommunicationMessage = communicationMessage;
  }

  public CommunicationMessageRecord CommunicationMessage { get; set; }
}
