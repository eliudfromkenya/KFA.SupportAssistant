using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_communication_messages")]
public sealed record class CommunicationMessage : BaseModel
{
  public override object ToBaseDTO()
  {
    return (CommunicationMessageDTO)this;
  }
  public override string? ___tableName___ { get; protected set; } = "tbl_communication_messages";
  [Column("attachments")]
  public byte[]? Attachments { get; init; }

  [MaxLength(255, ErrorMessage = "Please details must be 255 characters or less")]
  [Encrypted]
  [Column("details")]
  public string? Details { get; init; } = string.Empty;

  [MaxLength(255, ErrorMessage = "Please from must be 255 characters or less")]
  [Encrypted]
  [Column("from")]
  public string? From { get; init; } = string.Empty;

  // [Required]
  [Encrypted]
  [MaxLength(255, ErrorMessage = "Please message must be 255 characters or less")]
  [Column("message")]
  public string? Message { get; init; } = string.Empty;

  // [Required]
  [Column("message_id")]
  public override string? Id { get; init; }

  // [Required]
  //[Encrypted]
  [MaxLength(255, ErrorMessage = "Please message type must be 255 characters or less")]
  [Column("message_type")]
  public CommunicationMessageType? MessageType { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Encrypted]
  [Column("narration")]
  public string? Narration { get; init; } = string.Empty;

  [MaxLength(255, ErrorMessage = "Please status must be 255 characters or less")]
  [Column("status")]
  public CommunicationMessageStatus? Status { get; init; }

  [MaxLength(255, ErrorMessage = "Please title must be 255 characters or less")]
  [Encrypted]
  [Column("title")]
  public string? Title { get; init; } = string.Empty;

  [MaxLength(255, ErrorMessage = "Please to must be 255 characters or less")]
  [Encrypted]
  [Column("to")]
  public string? To { get; init; } = string.Empty;
}
