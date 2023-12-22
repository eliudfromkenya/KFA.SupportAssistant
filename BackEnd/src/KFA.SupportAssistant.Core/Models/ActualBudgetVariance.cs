using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_actual_budget_variances")]
public sealed record class ActualBudgetVariance : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_actual_budget_variances";
  [Required]
  [Column("actual_budget_id")]
  public override string? Id { get; set; }

  [Required]
  [Column("actual_value")]
  public decimal ActualValue { get; init; }

  [Column("batch_key")]
  public long BatchKey { get; init; }

  [Column("budget_value")]
  public decimal BudgetValue { get; init; }

  [MaxLength(255, ErrorMessage = "Please comment must be 255 characters or less")]
  [Column("comment")]
  public string? Comment { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(255, ErrorMessage = "Please field 1 must be 255 characters or less")]
  [Column("field_1")]
  public string? Field1 { get; init; }

  [MaxLength(255, ErrorMessage = "Please field 2 must be 255 characters or less")]
  [Column("field_2")]
  public string? Field2 { get; init; }

  [MaxLength(255, ErrorMessage = "Please field 3 must be 255 characters or less")]
  [Column("field_3")]
  public string? Field3 { get; init; }

  [Column("ledger_code")]
  public string? LedgerCode { get; init; }

  [Column("ledger_cost_centre_code")]
  public string? LedgerCostCentreCode { get; init; }

  [ForeignKey(nameof(LedgerCostCentreCode))]
  public CostCentre? LedgerCostCentre { get; set; }
  [NotMapped]
  public string? LedgerCostCentre_Caption { get; set; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  public ICollection<LedgerAccount>? LedgerAccounts { get; set; }
  public override object ToBaseDTO()
  {
    return (ActualBudgetVarianceDTO)this;
  }
}
