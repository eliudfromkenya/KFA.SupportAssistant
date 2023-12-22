using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

public record UpdatePayrollGroupRequest
{
  public const string Route = "/payroll_groups/{groupID}";
  [Required]
  public string? GroupID { get; set; }
  [Required]
  public string? GroupName { get; set; }
  public string? Narration { get; set; }
}
