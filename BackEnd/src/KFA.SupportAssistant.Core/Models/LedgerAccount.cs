using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Globals;

namespace KFA.DynamicsAssistant.Infrastructure.Models;
[Table("tbl_ledger_accounts")]
internal sealed record class LedgerAccount : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_ledger_accounts";
  [Column("cost_centre_code")]
  public string? CostCentreCode { get; init; }

  [ForeignKey(nameof(CostCentreCode))]
  public CostCentre? CostCentre { get; set; }

  public string? CostCentre_Caption { get; set; }

  [Required]
  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(255, ErrorMessage = "Please group name must be 255 characters or less")]
  [Column("group_name")]
  public string? GroupName { get; init; }

  [Required]
  [Column("increase_with_debit")]
  public bool IncreaseWithDebit { get; init; }

  [Required]
  [Column("ledger_account_code")]
  public string? LedgerAccountCode { get; init; }

  [ForeignKey(nameof(LedgerAccountCode))]
  public LeasedPropertiesAccount? LeasedPropertiesAccount { get; set; }

  public string? LedgerAccount_Caption { get; set; }

  [Required]
  [Column("ledger_account_id")]
  public override string? Id { get; set; }

  [MaxLength(255, ErrorMessage = "Please main group must be 255 characters or less")]
  [Column("main_group")]
  public string? MainGroup { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  public Supplier? Supplier { get; set; }
  public string? Supplier_Caption { get; set; }
}
