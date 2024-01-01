using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class CommunicationMessageDTO : BaseDTO<CommunicationMessage>
{
  public byte[]? Attachments { get; set; }
  public string? Details { get; set; }
  public string? From { get; set; }
  public string? Message { get; set; }
  public CommunicationMessageType? MessageType { get; set; }
  public string? Narration { get; set; }
  public CommunicationMessageStatus? Status { get; set; }
  public string? Title { get; set; }
  public string? To { get; set; }
  public override CommunicationMessage? ToModel()
  {
    return (CommunicationMessage)this;
  }

  public static implicit operator CommunicationMessageDTO(CommunicationMessage obj)
  {
    return new CommunicationMessageDTO
    {
      Attachments = obj.Attachments,
      Details = obj.Details,
      From = obj.From,
      Message = obj.Message,
      MessageType = obj.MessageType,
      Narration = obj.Narration,
      Status = obj.Status,
      Title = obj.Title,
      To = obj.To,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator CommunicationMessage(CommunicationMessageDTO obj)
  {
    return new CommunicationMessage
    {
      Attachments = obj.Attachments,
      Details = obj?.Details??string.Empty,
      From = obj?.From ?? string.Empty,
      Message = obj?.Message ?? string.Empty,
      MessageType = obj?.MessageType,
      Narration = obj?.Narration ?? string.Empty,
      Status = obj?.Status,
      Title = obj?.Title ?? string.Empty,
      To = obj?.To ?? string.Empty,
      Id = obj?.Id ?? string.Empty,
      ___DateInserted___ = obj?.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj?.DateUpdated___.FromDateTime()
    };
  }
}
