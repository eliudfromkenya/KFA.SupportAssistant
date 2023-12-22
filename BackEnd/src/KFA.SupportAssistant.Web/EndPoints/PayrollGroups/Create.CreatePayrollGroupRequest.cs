using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

public class CreatePayrollGroupRequest
{
  public const string Route = "/payroll_groups";

  [Required]
  public string? GroupID { get; set; }

  [Required]
  public string? GroupName { get; set; }

  public string? Narration { get; set; }
}
