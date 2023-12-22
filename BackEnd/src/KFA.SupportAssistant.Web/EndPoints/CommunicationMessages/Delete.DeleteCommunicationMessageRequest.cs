namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

public record DeleteCommunicationMessageRequest
{
  public const string Route = "/communication_messages/{messageID}";
  public static string BuildRoute(string? messageID) => Route.Replace("{messageID}", messageID);
  public string? MessageID { get; set; }
}
