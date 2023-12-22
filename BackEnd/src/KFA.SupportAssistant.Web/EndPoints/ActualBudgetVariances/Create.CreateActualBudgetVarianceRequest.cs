using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

public class CreateActualBudgetVarianceRequest
{
  public const string Route = "/actual_budget_variances";

  [Required]
  public string? ActualBudgetID { get; set; }

  [Required]
  public decimal? ActualValue { get; set; }

  public string? BatchKey { get; set; }
  public decimal? BudgetValue { get; set; }
  public string? Comment { get; set; }
  public string? Description { get; set; }
  public string? Field1 { get; set; }
  public string? Field2 { get; set; }
  public string? Field3 { get; set; }
  public string? LedgerCode { get; set; }
  public string? LedgerCostCentreCode { get; set; }
  public string? Narration { get; set; }
}
