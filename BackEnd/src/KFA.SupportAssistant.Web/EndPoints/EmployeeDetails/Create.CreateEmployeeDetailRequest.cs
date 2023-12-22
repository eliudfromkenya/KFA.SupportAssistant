using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.EmployeeDetails;

public class CreateEmployeeDetailRequest
{
  public const string Route = "/employee_details";
  public decimal? AmountDue { get; set; }
  public string? Classification { get; set; }
  public string? CostCentreCode { get; set; }
  public global::System.DateTime? Date { get; set; }
  public string? Email { get; set; }

  [Required]
  public string? EmployeeID { get; set; }

  [Required]
  public string? FullName { get; set; }

  public string? Gender { get; set; }
  public string? GroupNumber { get; set; }
  public string? IdNumber { get; set; }
  public string? Narration { get; set; }
  public string? PayrollGroupID { get; set; }
  public string? PayrollNumber { get; set; }
  public string? PhoneNumber { get; set; }
  public global::System.DateTime? RejoinDate { get; set; }
  public string? Remarks { get; set; }
  public decimal? RetireeAmount { get; set; }
  public decimal? RetrenchmentAmount { get; set; }
  public global::System.DateTime? RetrenchmentDate { get; set; }
  public string? Status { get; set; }
}
