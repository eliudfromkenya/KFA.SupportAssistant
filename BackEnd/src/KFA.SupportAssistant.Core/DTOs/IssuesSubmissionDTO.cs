using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Core.DTOs;
public record class IssuesSubmissionDTO : BaseDTO<IssuesSubmission>
{
  public string? IssueID { get; set; }
  public string? Narration { get; set; }
  public IssueStatus? Status { get; set; }
  public string? SubmittedTo { get; set; }
  public string? SubmittingUser { get; set; }
  public DateTime? TimeSubmitted { get; set; }
  public string? Type { get; set; }
  public override IssuesSubmission? ToModel()
  {
    return (IssuesSubmission)this;
  }
  public static implicit operator IssuesSubmissionDTO(IssuesSubmission obj)
  {
    return new IssuesSubmissionDTO
    {
      IssueID = obj.IssueID,
      Narration = obj.Narration,
      Status = obj.Status,
      SubmittedTo = obj.SubmittedTo,
      SubmittingUser = obj.SubmittingUser,
      TimeSubmitted = obj.TimeSubmitted,
      Type = obj.Type,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator IssuesSubmission(IssuesSubmissionDTO obj)
  {
    return new IssuesSubmission
    {
      IssueID = obj.IssueID,
      Narration = obj.Narration,
      Status = obj.Status,
      SubmittedTo = obj.SubmittedTo,
      SubmittingUser = obj.SubmittingUser,
      TimeSubmitted = obj.TimeSubmitted,
      Type = obj.Type,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
