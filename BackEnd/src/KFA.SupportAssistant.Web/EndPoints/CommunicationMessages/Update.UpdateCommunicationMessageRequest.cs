using System.ComponentModel.DataAnnotations;
using KFA.SupportAssistant.Core.DataLayer.Types;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

public record UpdateCommunicationMessageRequest
{
  public const string Route = "/communication_messages/{messageID}";
  public byte[]? Attachments { get; set; }
  public string? Details { get; set; }
  public string? From { get; set; }
  [Required]
  public string? Message { get; set; }
  [Required]
  public string? MessageID { get; set; }
  [Required]
  public CommunicationMessageType? MessageType { get; set; }
  public string? Narration { get; set; }
  public CommunicationMessageStatus? Status { get; set; }
  public string? Title { get; set; }
  public string? To { get; set; }
}
