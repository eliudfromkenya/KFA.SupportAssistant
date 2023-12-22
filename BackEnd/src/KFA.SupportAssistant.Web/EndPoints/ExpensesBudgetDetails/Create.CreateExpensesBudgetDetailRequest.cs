using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ExpensesBudgetDetails;

public class CreateExpensesBudgetDetailRequest
{
  public const string Route = "/expenses_budget_details";
  public string? BasisOfCalculation { get; set; }
  public string? BatchKey { get; set; }

  [Required]
  public string? ExpenseBudgetDetailId { get; set; }

  public string? LedgerAccountCode { get; set; }
  public decimal? Month01 { get; set; }
  public decimal? Month02 { get; set; }
  public decimal? Month03 { get; set; }
  public decimal? Month04 { get; set; }
  public decimal? Month05 { get; set; }
  public decimal? Month06 { get; set; }
  public decimal? Month07 { get; set; }
  public decimal? Month08 { get; set; }
  public decimal? Month09 { get; set; }
  public decimal? Month10 { get; set; }
  public decimal? Month11 { get; set; }
  public decimal? Month12 { get; set; }
  public string? Narration { get; set; }
}
