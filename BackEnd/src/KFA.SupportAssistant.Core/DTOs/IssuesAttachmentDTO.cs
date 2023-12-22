using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class IssuesAttachmentDTO : BaseDTO<IssuesAttachment>
{
  public string? AttachmentType { get; set; }
  public byte[]? Data { get; set; }
  public string? Description { get; set; }
  public string? File { get; set; }
  public string? IssueID { get; set; }
  public string? Narration { get; set; }
  public global::System.DateTime Time { get; set; }
  public override IssuesAttachment? ToModel()
  {
    return (IssuesAttachment)this;
  }

  public static implicit operator IssuesAttachmentDTO(IssuesAttachment obj)
  {
    return new IssuesAttachmentDTO
    {
      AttachmentType = obj.AttachmentType,
      Data = obj.Data,
      Description = obj.Description,
      File = obj.File,
      IssueID = obj.IssueID,
      Narration = obj.Narration,
      Time = obj.Time,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator IssuesAttachment(IssuesAttachmentDTO obj)
  {
    return new IssuesAttachment
    {
      AttachmentType = obj.AttachmentType,
      Data = obj.Data,
      Description = obj.Description,
      File = obj.File,
      IssueID = obj.IssueID,
      Narration = obj.Narration,
      Time = obj.Time,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
