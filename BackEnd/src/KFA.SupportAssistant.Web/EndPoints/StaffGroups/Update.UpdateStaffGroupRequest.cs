using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

public record UpdateStaffGroupRequest
{
  public const string Route = "/staff_groups/{groupNumber}";
  [Required]
  public string? Description { get; set; }
  [Required]
  public string? GroupNumber { get; set; }
  [Required]
  public bool? IsActive { get; set; }
  public string? Narration { get; set; }
}
