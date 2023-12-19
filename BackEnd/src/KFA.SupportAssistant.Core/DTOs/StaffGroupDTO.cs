using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class StaffGroupDTO : BaseDTO<StaffGroup>
{
  public string? Description { get; set; }
  public bool IsActive { get; set; }
  public string? Narration { get; set; }
  public override StaffGroup? ToModel()
  {
    return (StaffGroup)this;
  }

  public static implicit operator StaffGroupDTO(StaffGroup obj)
  {
    return new StaffGroupDTO
    {
      Description = obj.Description,
      IsActive = obj.IsActive,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator StaffGroup(StaffGroupDTO obj)
  {
    return new StaffGroup
    {
      Description = obj.Description,
      IsActive = obj.IsActive,
      Narration = obj.Narration,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
