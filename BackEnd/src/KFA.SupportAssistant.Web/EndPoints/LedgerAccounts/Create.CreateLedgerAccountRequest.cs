using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

public class CreateLedgerAccountRequest
{
  public const string Route = "/ledger_accounts";
  public string? CostCentreCode { get; set; }

  [Required]
  public string? Description { get; set; }

  public string? GroupName { get; set; }

  [Required]
  public bool? IncreaseWithDebit { get; set; }

  [Required]
  public string? LedgerAccountCode { get; set; }

  [Required]
  public string? LedgerAccountId { get; set; }

  public string? MainGroup { get; set; }
  public string? Narration { get; set; }
}
