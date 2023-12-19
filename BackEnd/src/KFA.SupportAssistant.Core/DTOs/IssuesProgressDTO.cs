using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Core.DTOs;
public record class IssuesProgressDTO : BaseDTO<IssuesProgress>
{
  public string? Description { get; set; }
  public string? IssueID { get; set; }
  public string? Narration { get; set; }
  public string? ReportedBy { get; set; }
  public IssueStatus? Status { get; set; }
  public global::System.DateTime Time { get; set; }
  public override IssuesProgress? ToModel()
  {
    return (IssuesProgress)this;
  }

  public static implicit operator IssuesProgressDTO(IssuesProgress obj)
  {
    return new IssuesProgressDTO
    {
      Description = obj.Description,
      IssueID = obj.IssueID,
      Narration = obj.Narration,
      ReportedBy = obj.ReportedBy,
      Status = obj.Status,
      Time = obj.Time,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator IssuesProgress(IssuesProgressDTO obj)
  {
    return new IssuesProgress
    {
      Description = obj.Description,
      IssueID = obj.IssueID,
      Narration = obj.Narration,
      ReportedBy = obj.ReportedBy,
      Status = obj.Status,
      Time = obj.Time,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }

}
