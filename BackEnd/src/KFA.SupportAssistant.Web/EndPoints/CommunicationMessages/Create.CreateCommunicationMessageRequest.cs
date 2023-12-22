using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

public class CreateCommunicationMessageRequest
{
  public const string Route = "/communication_messages";
  public byte[]? Attachments { get; set; }
  public string? Details { get; set; }
  public string? From { get; set; }

  [Required]
  public string? Message { get; set; }

  [Required]
  public string? MessageID { get; set; }

  [Required]
  public string? MessageType { get; set; }

  public string? Narration { get; set; }
  public string? Status { get; set; }
  public string? Title { get; set; }
  public string? To { get; set; }
}
