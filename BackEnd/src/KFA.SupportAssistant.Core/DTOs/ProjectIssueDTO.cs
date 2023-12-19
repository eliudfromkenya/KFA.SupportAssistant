using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Models.Types;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ProjectIssueDTO : BaseDTO<ProjectIssue>
{
  public string? Category { get; set; }
  public global::System.DateTime Date { get; set; }
  public string? Description { get; set; }
  public string? Effect { get; set; }
  public string? Narration { get; set; }
  public string? RegisteredBy { get; set; }
  public IssueStatus Status { get; set; }
  public string? SubCategory { get; set; }
  public string? Title { get; set; }
  public override ProjectIssue? ToModel()
  {
    return (ProjectIssue)this;
  }

  public static implicit operator ProjectIssueDTO(ProjectIssue obj)
  {
    return new ProjectIssueDTO
    {
      Category = obj.Category,
      Date = obj.Date,
      Description = obj.Description,
      Effect = obj.Effect,
      Narration = obj.Narration,
      RegisteredBy = obj.RegisteredBy,
      Status = obj.Status,
      SubCategory = obj.SubCategory,
      Title = obj.Title,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ProjectIssue(ProjectIssueDTO obj)
  {
    return new ProjectIssue
    {
      Category = obj.Category,
      Date = obj.Date,
      Description = obj.Description,
      Effect = obj.Effect,
      Narration = obj.Narration,
      RegisteredBy = obj.RegisteredBy,
      Status = obj.Status,
      SubCategory = obj.SubCategory,
      Title = obj.Title,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
