using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

public class CreateStaffGroupRequest
{
  public const string Route = "/staff_groups";

  [Required]
  public string? Description { get; set; }

  [Required]
  public string? GroupNumber { get; set; }

  [Required]
  public bool? IsActive { get; set; }

  public string? Narration { get; set; }
}
