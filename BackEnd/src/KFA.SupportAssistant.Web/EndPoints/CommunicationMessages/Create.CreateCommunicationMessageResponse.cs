using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

public readonly struct CreateCommunicationMessageResponse(byte[]? attachments, string? details, string? from, string? message, string? messageID, CommunicationMessageType? messageType, string? narration, CommunicationMessageStatus? status, string? title, string? to, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public byte[]? Attachments { get; } = attachments;
  public string? Details { get; } = details;
  public string? From { get; } = from;
  public string? Message { get; } = message;
  public string? MessageID { get; } = messageID;
  public CommunicationMessageType? MessageType { get; } = messageType;
  public string? Narration { get; } = narration;
  public CommunicationMessageStatus? Status { get; } = status;
  public string? Title { get; } = title;
  public string? To { get; } = to;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
